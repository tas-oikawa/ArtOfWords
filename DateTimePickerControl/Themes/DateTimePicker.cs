using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AC.AvalonControlsLibrary.Core;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// DateTimePicker Control to select date and time
    /// </summary>
    [TemplatePart(Name="PART_DatePicker", Type=typeof(DatePicker))]
    [TemplatePart(Name="PART_TimePicker", Type=typeof(TimePicker))]
    public class DateTimePicker : Control
    {
        #region Members
        DatePicker datePicker;
        TimePicker timePicker;
        #endregion

        #region Properties

        #region UI Design Properties
        /// <summary>
        /// Gets or sets the color to use for the minute hand
        /// </summary>
        public Brush MinuteHand
        {
            get { return (Brush)GetValue(MinuteHandProperty); }
            set { SetValue(MinuteHandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to use for the minute hand
        /// </summary>
        public static readonly DependencyProperty MinuteHandProperty =
            DependencyProperty.Register("MinuteHand", typeof(Brush), typeof(DateTimePicker), new UIPropertyMetadata(Brushes.White));

        /// <summary>
        /// Gets or sets the color to use for the hour hand
        /// </summary>
        public Brush HourHand
        {
            get { return (Brush)GetValue(HourHandProperty); }
            set { SetValue(HourHandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to use for the hour hand
        /// </summary>
        public static readonly DependencyProperty HourHandProperty =
            DependencyProperty.Register("HourHand", typeof(Brush), typeof(DateTimePicker), new UIPropertyMetadata(Brushes.White));

        /// <summary>
        /// Gets or sets the backgroud to use for the clock
        /// </summary>
        public Brush ClockBackground
        {
            get { return (Brush)GetValue(ClockBackgroundProperty); }
            set { SetValue(ClockBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the backgroud to use for the clock
        /// </summary>
        public static readonly DependencyProperty ClockBackgroundProperty =
            DependencyProperty.Register("ClockBackground", typeof(Brush), typeof(DateTimePicker), new UIPropertyMetadata(Brushes.Silver));

        /// <summary>
        /// Gets or sets the background color used by the header of the Calander
        /// </summary>
        public Brush CalanderHeaderBackground
        {
            get { return (Brush)GetValue(CalanderHeaderBackgroundProperty); }
            set { SetValue(CalanderHeaderBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CalanderHeaderBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalanderHeaderBackgroundProperty =
            DependencyProperty.Register("CalanderHeaderBackground", typeof(Brush), typeof(DateTimePicker), new UIPropertyMetadata(Brushes.Silver));


        /// <summary>
        /// Gets or sets the calander foreground for the text
        /// </summary>
        public Brush CalanderHeaderForeground
        {
            get { return (Brush)GetValue(CalanderHeaderForegroundProperty); }
            set { SetValue(CalanderHeaderForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the calander foreground for the text
        /// </summary>
        public static readonly DependencyProperty CalanderHeaderForegroundProperty =
            DependencyProperty.Register("CalanderHeaderForeground", typeof(Brush), typeof(DateTimePicker), new UIPropertyMetadata(Brushes.White));

        #endregion

        /// <summary>
        /// Gets or sets the max date to be set for the DatePicker
        /// </summary>
        public DateTime MaxDate
        {
            get { return (DateTime)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the max date to be set for the DatePicker
        /// </summary>
        public static readonly DependencyProperty MaxDateProperty =
            DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(DateTimePicker),
            new UIPropertyMetadata(DateTime.MaxValue));


        /// <summary>
        /// The Minimum date for the date picker
        /// </summary>
        public DateTime MinDate
        {
            get { return (DateTime)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        /// <summary>
        /// The Minimum date for the date picker
        /// </summary>
        public static readonly DependencyProperty MinDateProperty =
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(DateTimePicker),
            new UIPropertyMetadata(DateTime.MinValue));

        /// <summary>
        /// Gets or sets the DateTime Selected
        /// </summary>
        public DateTime DateTimeSelected
        {
            get { return (DateTime)GetValue(DateTimeSelectedProperty); }
            set { SetValue(DateTimeSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DateTime Selected
        /// </summary>
        public static readonly DependencyProperty DateTimeSelectedProperty =
            DependencyProperty.Register("DateTimeSelected", typeof(DateTime), typeof(DateTimePicker), new UIPropertyMetadata(DateTime.Now,
                new PropertyChangedCallback(DateTimeSelectedPropertyChanged)));

        #endregion

        #region Events

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateTimeChanged",
            RoutingStrategy.Bubble, typeof(DateTimeSelectedChangedRoutedEventHandler), typeof(DateTimePicker));

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public event DateTimeSelectedChangedRoutedEventHandler SelectedDateTimeChanged
        {
            add { AddHandler(SelectedDateTimeChangedEvent, value); }
            remove { RemoveHandler(SelectedDateTimeChangedEvent, value); }
        }

        #endregion

        #region PropertyChanged Handlers
        bool isDateTimeBeingUpdated = false;
        protected static void DateTimeSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker dateTimePicker = (DateTimePicker)sender;
            dateTimePicker.isDateTimeBeingUpdated = true;
            if (dateTimePicker.datePicker != null && dateTimePicker.timePicker != null)
            {
                dateTimePicker.datePicker.CurrentlySelectedDate = dateTimePicker.DateTimeSelected;
                dateTimePicker.timePicker.SelectedTime = dateTimePicker.DateTimeSelected.TimeOfDay;
                dateTimePicker.OnDateTimeSelectedChanged((DateTime)e.NewValue, (DateTime)e.OldValue);
            }
            dateTimePicker.isDateTimeBeingUpdated = false;
        }
        #endregion

        /// <summary>
        /// static constructor for the DatetimePicker
        /// </summary>
        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
               typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)
                   ));
        }

        /// <summary>
        /// Override the default template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            datePicker = (DatePicker)GetTemplateChild("PART_DatePicker");
            if (datePicker == null) datePicker = new DatePicker();
            timePicker = (TimePicker)GetTemplateChild("PART_TimePicker");
            if (timePicker == null) timePicker = new TimePicker();
            datePicker.SelectedDateChanged += delegate{ SetCurrentDateTime(); };
            timePicker.SelectedTimeChanged += delegate { SetCurrentDateTime(); };
            datePicker.CurrentlySelectedDate = DateTimeSelected;
            timePicker.SelectedTime = DateTimeSelected.TimeOfDay;
            
            //snyc the min and max date for datepicker
            Binding minDateBinding = new Binding("MinDate");
            minDateBinding.Source = this;
            datePicker.SetBinding(DatePicker.MinDateProperty, minDateBinding);
            Binding maxDateBinding = new Binding("MaxDate");
            maxDateBinding.Source = this;
            datePicker.SetBinding(DatePicker.MaxDateProperty, maxDateBinding);

            //snyc the min and max time for timepicker
            Binding minTimeBinding = new Binding("MinDate.TimeOfDay");
            minTimeBinding.Source = this;
            timePicker.SetBinding(TimePicker.MinTimeProperty, minTimeBinding);
            Binding maxTimeBinding = new Binding("MaxDate.TimeOfDay");
            maxTimeBinding.Source = this;
            timePicker.SetBinding(TimePicker.MaxTimeProperty, maxTimeBinding);
            
        }

        #region Helper methods
        private void SetCurrentDateTime()
        {
            if (!isDateTimeBeingUpdated)
            {
                DateTimeSelected = new DateTime(
                    datePicker.CurrentlySelectedDate.Year, datePicker.CurrentlySelectedDate.Month, datePicker.CurrentlySelectedDate.Day,
                    timePicker.SelectedHour, timePicker.SelectedMinute, timePicker.SelectedSecond
                    );
            }
        }
        protected void OnDateTimeSelectedChanged(DateTime newDateTime, DateTime oldDateTime)
        {
            DateTimeSelectedChangedRoutedEventArgs args = 
                new DateTimeSelectedChangedRoutedEventArgs(SelectedDateTimeChangedEvent);
            args.NewDate = newDateTime;
            args.OldDate = oldDateTime;
            RaiseEvent(args);
        }
        #endregion

        #region UnitTests
        /// <summary>
        /// Exposes the DatePicker and Time picker for unit tests
        /// </summary>
        /// <param name="datePicker"></param>
        /// <param name="timePicker"></param>
        [System.Diagnostics.Conditional(Globals.UnitTestSymbol)]
        public void ExposedDatePicker(ref DatePicker datePicker, ref TimePicker timePicker)
        {
            datePicker = this.datePicker;
            timePicker = this.timePicker;
        }
        #endregion
    }

    #region DateTime RoutedEvent
    /// <summary>
    /// Delegate for the date selected changed
    /// </summary>
    /// <param name="sender">The object raising the event</param>
    /// <param name="e">The event arguments for event being raised</param>
    public delegate void DateTimeSelectedChangedRoutedEventHandler(object sender, DateTimeSelectedChangedRoutedEventArgs e);

    /// <summary>
    /// Event arguments for the DateTimeSelected changed
    /// </summary>
    public class DateTimeSelectedChangedRoutedEventArgs : DateSelectedChangedRoutedEventArgs
    {
        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="routedEvent">The event raised</param>
        public DateTimeSelectedChangedRoutedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent) { }
    }
    #endregion

    #region Converters

    //gets the length that the minute/hour hand should use
    public class HandLengthConverter : IMultiValueConverter
    {
        /// <summary>
        /// Gets or sets the percentage of the length to show
        /// </summary>
        public double PercentageToSubtract { get; set; }

        #region IMultiValueConverter Members

        public object  Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
 	        //value 1 is the actual height
            double height = (double)values[0];
            //value 2 is the actual width
            double width = (double)values[1];
            
            //select the shortest
            double valueToUse = (height > width ? width : height) / 2;
            double percent = (PercentageToSubtract / 100) * valueToUse;
            
            if (valueToUse == 0)
                return 0.0;

            return valueToUse - percent;
        }

        public object[]  ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
 	        throw new NotImplementedException();
        }

        #endregion
    }

    //gets the actual angle to rotate the minute/hour hand
    public class AngleConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the number of units to calculate the angle on
        /// </summary>
        public int Units { get; set; }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)value * (360 / Units)) - 90;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Places the minute hand and the hour hand in the center of the canvas
    /// </summary>
    public class AlignClockHands : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (((double)value) / 2) ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    #endregion

}
