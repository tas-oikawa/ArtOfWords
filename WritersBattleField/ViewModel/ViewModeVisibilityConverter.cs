using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace WritersBattleField.ViewModel
{
    public class ViewModeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ViewMode target = (ViewMode)value;

            switch (target)
            {
                case ViewMode.Writing:
                    return Visibility.Hidden;
                case ViewMode.Character:
                    return Visibility.Visible;
                case ViewMode.StoryFrame:
                    return Visibility.Visible;

                default:
                    throw new NotImplementedException("実装されてないインデックスです");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("実装されません");
        }
    }
}
