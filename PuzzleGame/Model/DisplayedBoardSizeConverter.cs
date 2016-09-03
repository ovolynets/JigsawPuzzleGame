using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace PuzzleGame.Model
{
    public class DisplayedBoardSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var entry = value as Tuple<int, int>;
            if (entry == null)
            {
                return null;
            }

            return string.Format("{0} ({1}x{2})", entry.Item1 * entry.Item2, entry.Item1, entry.Item2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}