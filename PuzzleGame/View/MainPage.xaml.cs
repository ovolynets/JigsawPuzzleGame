using System;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using PuzzleGame.Model;

namespace PuzzleGame.View
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private static async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var properties = ApplicationData.Current.LocalSettings.Values;
            if (properties.ContainsKey("soundEnabled"))
            {
                GameSettings.IsSoundEnabled = (bool)properties["soundEnabled"];
            }

            if (properties.ContainsKey("boardSizeIndex"))
            {
                GameSettings.SelectedBoardSizeIndex = (int) properties["boardSizeIndex"];
            }
            if (properties.ContainsKey("imageDirectory") && DataService.GalleryFolder == null)
            {
                var f = await StorageFolder.GetFolderFromPathAsync((string)ApplicationData.Current.LocalSettings.Values["imageDirectory"]);
                if (f != null)
                {
                    DataService.GalleryFolder = f;
                }

            }

        }

        private void NewGameButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(FileListPage));
        }

        private void SettingsButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof (SettingsPage));
        }

        private void ExitGameButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void AboutButton_Pressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof (AboutPage));
        }
    }
}
