using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace PuzzleGame.Model
{
    public class MyImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var imageFile = value as ImageFile;
            if (imageFile == null) return null;
            if (DataService.GalleryFolder.TryGetItemAsync(imageFile.FileName).AsTask().Result == null)
            {
                return null;
            }
            var file = DataService.GalleryFolder.GetFileAsync(imageFile.FileName).AsTask().Result;
            var stream = file.OpenReadAsync().AsTask().Result;
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}