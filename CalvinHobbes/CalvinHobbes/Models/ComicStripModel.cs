using CalvinHobbes.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CalvinHobbes.Models
{
    public static class ComicStripModel
    {
        private static ObservableCollection<ComicStrip> comicStrips = new ObservableCollection<ComicStrip>();

        public static ObservableCollection<ComicStrip> ComicStrips
        {
            get
            {
                return comicStrips;
            }
            set
            {
                comicStrips = value;
            }
        }

        public static SortedDictionary<DateTime, ComicStrip> AllComicStrips = new SortedDictionary<DateTime, ComicStrip>();

        public static async Task Load()
        {
            await PopulateAllComicStrips();
        }

        public static async Task Save()
        {
            await SaveAllComicStrips();
        }

        public static async Task ResolveComicForDate(DateTime targetDate)
        {
            if (AllComicStrips.ContainsKey(targetDate))
            {
                ComicStrips.Add(ComicStripModel.AllComicStrips[targetDate]);
            }
            else
            {
                var comic = await Helper.GetComicStripAsync(targetDate);
                ComicStrips.Add(comic);
                ComicStripModel.AllComicStrips.Add(comic.Date, comic);
            }
        }

        private static async Task SaveAllComicStrips()
        {
            StringBuilder sb = new StringBuilder();

            foreach(var pair  in AllComicStrips)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3}",
                    pair.Value.Date.ToString(Constants.DATETIME_FORMAT),
                    pair.Value.ImageUrl,
                    pair.Value.AdjustedWidth / pair.Value.MultiplyingFactor,
                    pair.Value.AdjustedHeight / pair.Value.MultiplyingFactor));
            }

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(Constants.DATA_FILE_NAME, CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, sb.ToString());
        }

        private static async Task PopulateAllComicStrips()
        {
            var content = await ReadComicStripDataFromFile();

            foreach(var line in content)
            {
                string[] tokens = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                DateTime date = DateTime.ParseExact(tokens[0], Constants.DATETIME_FORMAT, null);
                string imageUrl = tokens[1];
                int width = int.Parse(tokens[2]);
                int height = int.Parse(tokens[3]);

                if (!AllComicStrips.ContainsKey(date))
                {
                    AllComicStrips.Add(date, new ComicStrip(imageUrl, date, width, height));
                }
            }
        }

        private static async Task<IList<string>> ReadComicStripDataFromFile()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var dataFile = await localFolder.TryGetItemAsync(Constants.DATA_FILE_NAME);

            if (dataFile != null && dataFile.IsOfType(StorageItemTypes.File))
            {
                return await FileIO.ReadLinesAsync((IStorageFile)dataFile);
            }
            else
            {
                var content = await ReadPreinstalledComicData();

                Task.Run(async () =>
                    {
                        var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(Constants.DATA_FILE_NAME, CreationCollisionOption.FailIfExists);
                        await Windows.Storage.FileIO.WriteLinesAsync(file, content);
                    });

                return content;
            }
        }

        private static async Task<IList<string>> ReadPreinstalledComicData()
        {
            var installFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var file = await installFolder.GetFileAsync(Constants.DATA_FILE_NAME);
            var content = await Windows.Storage.FileIO.ReadLinesAsync(file);

            return content;
        }
    }
}
