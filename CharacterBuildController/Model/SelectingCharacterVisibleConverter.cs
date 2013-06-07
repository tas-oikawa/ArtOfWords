using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using System.Windows;

namespace CharacterBuildControll.Model
{
    public class SelectingCharacterVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CharacterModel target = (CharacterModel)value;

            if (target == null)
            {
                return Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("実装されてないインデックスです");
        }
    }
}
