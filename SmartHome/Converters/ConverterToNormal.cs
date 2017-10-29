using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SmartHome.Converters
{
    public class ConverterToNormal : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            AccesLevel level = (AccesLevel)value;
            if (level == AccesLevel.Admin || level == AccesLevel.Normal)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
