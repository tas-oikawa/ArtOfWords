using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WritersBattleField.ViewModel
{
    public class ViewModeIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ViewMode target = (ViewMode)value;

            switch (target)
            {
                case ViewMode.Writing:
                    return 0;
                case ViewMode.Character:
                    return 1;
                case ViewMode.StoryFrame:
                    return 3;

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
                    return ViewMode.Writing;
                case 1:
                    return ViewMode.Character;
                case 3:
                    return ViewMode.StoryFrame;

                default:
                    throw new NotImplementedException("実装されてないインデックスです");
            }

        }
    }
}
