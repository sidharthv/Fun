using CalvinHobbes.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CalvinHobbes
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class FlipViewPage : Page
    {        
        public NavigationHelper NavigationHelper { get; set; }
        public FlipViewPage()
        {
            this.InitializeComponent();
            this.NavigationHelper = new NavigationHelper(this);
            this.NavigationHelper.SaveState += navigationHelper_SaveState;
            this.NavigationHelper.LoadState += navigationHelper_LoadState;
            this.Loaded += FlipViewPage_Loaded;

            datepicker.MinYear = new DateTimeOffset(Constants.MIN_COMIC_DATE);
            datepicker.MaxYear = DateTimeOffset.UtcNow;
        }

        void FlipViewPage_Loaded(object sender, RoutedEventArgs e)
        {            
            this.PART_BackButton.Command = NavigationHelper.GoBackCommand;
        }

        void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("FlipViewPageDataContext"))
            {
                this.DataContext = e.PageState["FlipViewPageDataContext"];
            }
            else
            {
                this.DataContext = new FlipViewPageViewModel();
            }
        }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["FlipViewPageDataContext"] = this.DataContext;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }
    }
}
