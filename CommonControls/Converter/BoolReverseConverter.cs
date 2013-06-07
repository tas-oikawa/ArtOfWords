using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CommonControls.Converter
{
    public class BoolReverseConverter : IValueConverter
    {
        #region IValueConverter メンバ

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool target = (bool)value;

            return !target;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool target = (bool)value;

            return !target;
        }

        #endregion
    }
}
