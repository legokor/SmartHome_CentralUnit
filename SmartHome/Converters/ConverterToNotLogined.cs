﻿
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    namespace SmartHome.Converters
    {
        public class ConverterToNotLogined : IValueConverter
        {

            public object Convert(object value, Type targetType, object parameter, string language)
            {
                return Visibility.Visible;
            }

            public object ConvertBack(object value, Type targetType, object parameter, string language)
            {
                return null;
            }
        }
    }

