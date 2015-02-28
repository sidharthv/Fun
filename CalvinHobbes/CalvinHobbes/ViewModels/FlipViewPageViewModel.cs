using CalvinHobbes.Common;
using CalvinHobbes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalvinHobbes
{
    public class FlipViewPageViewModel: INotifyPropertyChanged
    {
        private DateTimeOffset startDate = DateTimeOffset.Now.AddDays(0);
        private DateTimeOffset endDate = DateTimeOffset.Now;
        private ComicStrip selectedItem;

        public DateTimeOffset StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                OnPropertyChanged("StartDate");
            }
        }

        public DateTimeOffset EndDate
        {
            get
            {
                return endDate;
            }
            set
            {                
                endDate = value;
                OnPropertyChanged("EndDate");
            }
        }

        public ObservableCollection<ComicStrip> ComicStrips
        {
            get
            {
                return ComicStripModel.ComicStrips;
            }
        }

        public ComicStrip SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        RelayCommand refreshComicStripsCommand;
        public RelayCommand RefreshComicStripsCommand
        {
            get
            {
                if (null == refreshComicStripsCommand)
                {
                    refreshComicStripsCommand = new RelayCommand(
                        () =>
                        {
                            if (StartDate < Constants.MIN_COMIC_DATE || StartDate > DateTimeOffset.Now)
                            {
                                StartDate = DateTimeOffset.Now;
                                string message = string.Format("Comic strips are only available from {0} to the present day.", Constants.MIN_COMIC_DATE.ToString("d"));
                                Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog(message);
                                dialog.ShowAsync();
                            }
                            RefreshComicStripsAsync(StartDate.Date, EndDate.Date);
                        }, 
                        () =>
                        {
                            return false == this.RefreshInProgress;
                        });
                }

                return refreshComicStripsCommand;
            }
            set
            {
                refreshComicStripsCommand = value;
            }
        }

        bool refreshInProgress = false;

        public bool RefreshInProgress
        {
            get
            {
                return refreshInProgress;
            }
            set
            {
                refreshInProgress = value;
                RefreshComicStripsCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("RefreshInProgress");
            }
        }

        private async Task RefreshComicStripsAsync(DateTime startDate, DateTime endDate)
        {
            if (!RefreshInProgress)
            {
                RefreshInProgress = true;
                
                try
                {
                    ComicStrips.Clear();

                    // Resolve the first item so we can set the selected item
                    await ComicStripModel.ResolveComicForDate(startDate);
                    SelectedItem = ComicStrips.FirstOrDefault();

                    // Find/download the remaining items
                    DateTime indexDate = startDate.AddDays(1);
                    while (indexDate <= endDate)
                    {
                        await ComicStripModel.ResolveComicForDate(indexDate);
                        indexDate = indexDate.AddDays(1);
                    }

                    await ComicStripModel.Save();
                }
                finally
                {
                    RefreshInProgress = false;
                }
            }
        }

        public FlipViewPageViewModel()
        {
            RefreshComicStripsAsync(StartDate.Date, EndDate.Date);
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
