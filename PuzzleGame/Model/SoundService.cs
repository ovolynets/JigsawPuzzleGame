using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Search;

namespace PuzzleGame.Model
{
    public class SoundService
    {

        private static ObservableCollection<ImageFile> _imageFiles;

        public static ObservableCollection<ImageFile> LoadImages()
        {
            if (_imageFiles == null)
            {
                _imageFiles = new ObservableCollection<ImageFile>();
                var folder = KnownFolders.PicturesLibrary;
                if (folder != null)
                {
                    var fileList = folder.GetFilesAsync(CommonFileQuery.OrderByName).AsTask().Result;
                    foreach (var f in fileList)
                    {
                        _imageFiles.Add(new ImageFile { FileName = f.Name });
                    }
                }
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