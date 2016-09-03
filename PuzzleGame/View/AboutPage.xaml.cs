using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace PuzzleGame.View
{
    public sealed partial class AboutPage
    {
        public string GameName { get; set; }

        public string VersionNumber { get; set; }
        public AboutPage()
        {
            InitializeComponent();
            var version = Package.Current.Id.Version;
            VersionNumber = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private void ToggleLicensesView_OnClick(object sender, RoutedEventArgs e)
        {
            OpenSourceProjects.Visibility = Visibility.Collapsed;
            ToggleOpenSourceProjects.Text = "Show";
            if (Licenses.Visibility == Visibility.Collapsed)
            {
                Licenses.Visibility = Visibility.Visible;
                ToggleLicenses.Text = "Hide";
            }
            else
            {
                Licenses.Visibility = Visibility.Collapsed;
                ToggleLicenses.Text = "Show";
            }
        }

        private void ToggleOpenSourceProjects_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Licenses.Visibility = Visibility.Collapsed;
            ToggleLicenses.Text = "Show";
            if (OpenSourceProjects.Visibility == Visibility.Collapsed)
            {
                OpenSourceProjects.Visibility = Visibility.Visible;
                ToggleOpenSourceProjects.Text = "Hide";
            }
            else
            {
                OpenSourceProjects.Visibility = Visibility.Collapsed;
                ToggleOpenSourceProjects.Text = "Show";
            }
        }

        private void MainMenu_OnClick(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof (MainPage));
        }
    }
}
