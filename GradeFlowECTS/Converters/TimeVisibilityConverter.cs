using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GradeFlowECTS.Converters
{
    public class TimeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int minutes = (int)value;
            return minutes > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}