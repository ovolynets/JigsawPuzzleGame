using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Search;

namespace PuzzleGame.Model
{
    public class DataService
    {
        private static ObservableCollection<ImageFile> _imageFiles;
        private static StorageFolder _galleryFolder;

        public static StorageFolder GalleryFolder
        {
            get { return _galleryFolder ?? KnownFolders.PicturesLibrary; }
            set
            {
                _galleryFolder = value;
                ApplicationData.Current.LocalSettings.Values["imageDirectory"] = value.Path;
            }
        }

        public static ObservableCollection<ImageFile> LoadImages()
        {
            _imageFiles = new ObservableCollection<ImageFile>();

            if (_galleryFolder == null)
            {
                var installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
                _galleryFolder = installedLocation.GetFolderAsync("Assets\\sample").AsTask().Result;
            }

            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, GameSettings.ImageFileTypes);
            var fileList = _galleryFolder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync().AsTask().Result;
            foreach (var f in fileList)
            {
                _imageFiles.Add(new ImageFile { FileName = f.Name });
            }
            return _imageFiles;
        }

        public static ImageFile ChooseNext(ImageFile imageFile)
        {
            var index = _imageFiles.IndexOf(imageFile);
            var nextIndex = index >= _imageFiles.Count - 1 ? 0 : index + 1;
            return _imageFiles.ElementAt(nextIndex);
        }
    }
}
