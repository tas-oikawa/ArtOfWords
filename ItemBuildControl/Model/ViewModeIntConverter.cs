using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ItemBuildControl.Model
{
    public class ViewModeIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ViewMode target = (ViewMode)value;

            switch (target)
            {
                case ViewMode.File:
                    return 0;
                case ViewMode.Talking:
                    return 1;

                default:
                    throw new NotImplementedException("実装されてないインデックスです");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int target = (int)value;

            switch (target)
            {
                case 0:
                    return ViewMode.File;
                case 1:
                    return ViewMode.Talking;

                default:
                    throw new NotImplementedException("実装されてないインデックスです");
            }

        }
    }
}
