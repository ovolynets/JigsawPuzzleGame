using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using PuzzleGame.Annotations;
using PuzzleGame.Model;

namespace PuzzleGame.View
{
    public sealed partial class FileListPage : INotifyPropertyChanged
    {
        public ObservableCollection<ImageFile> ImageFiles { get; set; } = new ObservableCollection<ImageFile>();

        private string _text;

        public string StatusText
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }


        public FileListPage()
        {
            InitializeComponent();
            Loaded += FileListPage_Loaded;
        }

        private void FileListPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImages();
        }

        private void LoadImages()
        {
            ImageFiles = DataService.LoadImages();
            StatusText = "Loaded " + ImageFiles.Count + " files";
            Bindings.Update();
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var imageFile = e.ClickedItem as ImageFile;
            Frame.Navigate(typeof (GameBoardPage), imageFile);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OpenFolder_OnClick(object sender, RoutedEventArgs e)
        {
            var picker = new FolderPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var f = await picker.PickSingleFolderAsync();
            if (f != null)
            {
                DataService.GalleryFolder = f;
                LoadImages();
            }
        }

        private void MainMenu_OnClick(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof (MainPage));
        }
    }
}
