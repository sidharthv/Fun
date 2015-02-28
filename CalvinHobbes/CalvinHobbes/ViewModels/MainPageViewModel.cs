using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CalvinHobbes.Common;
using System.ComponentModel;
using CalvinHobbes.Models;

namespace CalvinHobbes
{
    public class MainPageViewModel : INotifyPropertyChanged
    {

        private ComicStrip todaysComicStrip;

        bool refreshInProgress = false;

        #region Properties
        public ComicStrip TodaysComicStrip
        {
            get
            {
                return todaysComicStrip;
            }
            set
            {
                todaysComicStrip = value;
                OnPropertyChanged("TodaysComicStrip");
            }
        }

        public bool RefreshInProgress
        {
            get
            {
                return refreshInProgress;
            }
            set
            {
                refreshInProgress = value;
                OnPropertyChanged("RefreshInProgress");
            }
        }
        #endregion

        public MainPageViewModel()
        {
            InitialLoad();
        }

        private async Task InitialLoad()
        {
            try
            {
                RefreshInProgress = true;
                await ComicStripModel.ResolveComicForDate(DateTime.Today);
                TodaysComicStrip = ComicStripModel.AllComicStrips[DateTime.Today];
            }
            finally
            {
                RefreshInProgress = false;
            }

            // Load and save
            // load takes care of bringing local data into memory
            // save takes care of writing today's comic back to file
            await ComicStripModel.Load();
            await ComicStripModel.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var propertyChangedHandler = PropertyChanged;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
