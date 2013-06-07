using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace AC.AvalonControlsLibrary.Core
{
    /// <summary>
    /// Base class for multivalue converters that do not implement the ConvertBack method
    /// </summary>
    public abstract class OneWayMultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts a value. Used in data binding. Override this method (sealed).
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the MultiBinding produces</param>
        /// <param name="targetType">The type of the binding target property</param>
        /// <param name="parameter">The converter parameter to use</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>A converted value</returns>
        public abstract object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        /// <summary>
        /// Unimplemented method for converting back. This converter can only be used for one-way binding.
        /// </summary>
        /// <param name="value">Unused parameter</param>
        /// <param name="targetTypes">Unused parameter</param>
        /// <param name="parameter">Unused parameter</param>
        /// <param name="culture">Unused parameter</param>
        /// <returns>Always throws an exception</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This is a one-way value converter. ConvertBack method is not supported.");
        }
    }

    /// <summary>
    /// Base class for value converters that do not implement the ConvertBack method.
    /// </summary>
    public abstract class OneWayValueConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value. Used in data binding. Override this method (sealed).
        /// Decorate the method with the type of return data (ValueConversion attribute).
        /// </summary>
        /// <param name="value">The value produced by the binding source</param>
        /// <param name="targetType">The type of the binding target property</param>
        /// <param name="parameter">The converter parameter to use</param>
        /// <param name="culture">The culture to use in the converter</param>
        /// <returns>A converted value</returns>
        public abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        /// <summary>
        /// Unimplemented method for converting back. This converter can only be used for one-way binding.
        /// </summary>
        /// <param name="value">Unused parameter</param>
        /// <param name="targetType">Unused parameter</param>
        /// <param name="parameter">Unused parameter</param>
        /// <param name="culture">Unused parameter</param>
        /// <returns>Always throws an exception</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("This is a one-way value converter. ConvertBack method is not supported.");
        }
    }
}
