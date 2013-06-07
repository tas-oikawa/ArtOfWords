using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TimelineControl.Converter
{
    public class StringIntConverter : IValueConverter
    {
        #region IValueConverter メンバ

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int target = (int)value;

            return target.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return int.Parse((string)value);
        }

        #endregion
    }
}
