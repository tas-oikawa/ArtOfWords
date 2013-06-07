using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using AC.AvalonControlsLibrary.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Security;
using System.Windows.Interop;
using System.Windows.Media;
using System.Collections.Specialized;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// Control to select a specific date
    /// </summary>
    [TemplatePart(Name = "PART_MonthBack", Type=typeof(ButtonBase)),
    TemplatePart(Name = "PART_MonthForward", Type = typeof(ButtonBase)),
    TemplatePart(Name = "PART_Dates", Type = typeof(Selector))]
    public class DatePicker : Control, INotifyPropertyChanged
    {
        #region const
        const string CurrentlyViewedMonthYearPropertyName = "CurrentlyViewedMonthYear";
        const string CurrentlySelectedDateStringPropertyName = "CurrentlySelectedDateString";
        const string CurrentlyViewedMonthPropertyName = "CurrentlyViewedMonth";
        const string CurrentlyViewedYearPropertyName = "CurrentlyViewedYear";
        #endregion

        //buttons for the back and forward
        private ButtonBase backButton, forwardButton;
        private Selector datesList;
        private DateHelper dateHelper = new DateHelper();

        //integer that stores the number of the month and year in view
        private int currentlyViewedMonth, currentlyViewedYear;

        #region properties and events

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
            DependencyProperty.Register("MaxDate", typeof(DateTime), typeof(DatePicker), 
            new UIPropertyMetadata(DateTime.MaxValue,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    DatePicker picker = (DatePicker)sender;
                    if (picker.MaxDate.Ticks < picker.CurrentlySelectedDate.Ticks)
                        picker.CurrentlySelectedDate = picker.MaxDate;

                    picker.ReBindListOfDays();
                }));


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
            DependencyProperty.Register("MinDate", typeof(DateTime), typeof(DatePicker), 
            new UIPropertyMetadata(DateTime.MinValue,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    DatePicker picker = (DatePicker)sender;
                    if (picker.MinDate.Ticks > picker.CurrentlySelectedDate.Ticks)
                        picker.CurrentlySelectedDate = picker.MinDate;

                    picker.ReBindListOfDays();
                }));

        /// <summary>
        /// Gets or sets the selection mode for the DatePicker
        /// </summary>
        public SelectionMode DatesSelectionMode
        {
            get { return (SelectionMode)GetValue(DatesSelectionModeProperty); }
            set { SetValue(DatesSelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection mode for the DatePicker
        /// </summary>
        public static readonly DependencyProperty DatesSelectionModeProperty =
            DependencyProperty.Register("DatesSelectionMode", typeof(SelectionMode), typeof(DatePicker),
            new UIPropertyMetadata(SelectionMode.Single, delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    if (((SelectionMode)e.NewValue) == SelectionMode.Multiple)
                        throw new NotSupportedException("SelectionMode.Multiple is not supported");
                }));

        /// <summary>
        /// Gets the list of months
        /// </summary>
        public static IEnumerable<string> MonthsList
        {
            get
            {
                return DateHelper.Months;
            }
        }

        /// <summary>
        /// Gets or sets the current month in view
        /// </summary>
        public int CurrentlyViewedMonth
        {
            get { return currentlyViewedMonth; }
            set 
            { 
                currentlyViewedMonth = value;
                ChangeDate();
            }
        }

        /// <summary>
        /// Gets or sets the current year in view
        /// </summary>
        public int CurrentlyViewedYear
        {
            get { return currentlyViewedYear; }
            set
            {
                currentlyViewedYear = value;
                ChangeDate();
            }
        }

        /// <summary>
        /// returns the month currently selected as a full string
        /// </summary>
        public string CurrentlyViewedMonthYear
        {
            get
            {
                return String.Format("{0} {1}",
                    DateHelper.GetMonthDisplayName(currentlyViewedMonth),
                    currentlyViewedYear);
            }
        }

        /// <summary>
        /// Gets and sets a string that represents the selected date
        /// </summary>
        public string CurrentlySelectedDateString
        {
            get
            {
                if (DatesSelectionMode != SelectionMode.Single)
                {
                    if(CurrentlySelectedDates.Count > 1)
                        return String.Format("{0} - {1}", CurrentlySelectedDates[0].ToShortDateString(),
                            CurrentlySelectedDates[CurrentlySelectedDates.Count - 1].ToShortDateString());
                    else if (CurrentlySelectedDates.Count == 1)
                        return CurrentlySelectedDates[0].ToString("yyyy/MM/dd");
                 }
                    
                return CurrentlySelectedDate.ToString("yyyy/MM/dd");
            }
        }

        /// <summary>
        /// Gets or sets a collection of selected dates
        /// This property can only be used if the DatesSelectionMode is not set to Single
        /// </summary>
        public ObservableCollection<DateTime> CurrentlySelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(CurrentlySelectedDatesProperty); }
            set { SetValue(CurrentlySelectedDatesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of selected dates
        /// This property can only be used if the DatesSelectionMode is not set to Single
        /// </summary>
        public static readonly DependencyProperty CurrentlySelectedDatesProperty =
            DependencyProperty.Register("CurrentlySelectedDates", typeof(ObservableCollection<DateTime>),
            typeof(DatePicker), new UIPropertyMetadata(null, delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    DatePicker picker = (DatePicker)sender;
                    INotifyCollectionChanged collection = e.NewValue as INotifyCollectionChanged;
                    if (collection != null)
                    {
                        collection.CollectionChanged += delegate
                        {
                            picker.OnPropertyChanged(new PropertyChangedEventArgs(CurrentlySelectedDateStringPropertyName));
                        };
                    }
                    picker.OnPropertyChanged(new PropertyChangedEventArgs(CurrentlySelectedDateStringPropertyName));
                }));


        /// <summary>
        /// Gets and sets the currently viewed date
        /// </summary>
        public DateTime CurrentlySelectedDate
        {
            get { return (DateTime)GetValue(CurrentlySelectedDateProperty); }
            set 
            { 
                SetValue(CurrentlySelectedDateProperty, value);
            }
        }

        /// <summary>
        /// Gets and sets the currently viewed date
        /// </summary>
        public static readonly DependencyProperty CurrentlySelectedDateProperty =
            DependencyProperty.Register("CurrentlySelectedDate", typeof(DateTime), typeof(DatePicker), 
            new UIPropertyMetadata(DateTime.Now, CurrentlySelectedDatePropertyChanged));

        //raise the property changed for CurrentlySelectedDateProperty
        static void CurrentlySelectedDatePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            DatePicker picker = (DatePicker)obj;
            picker.OnDateChanged(picker.CurrentlySelectedDate, (DateTime)e.OldValue);
            picker.OnPropertyChanged(new PropertyChangedEventArgs(CurrentlySelectedDateStringPropertyName));
        }

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", 
            RoutingStrategy.Bubble, typeof(DateSelectedChangedEventHandler), typeof(DatePicker));

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public event DateSelectedChangedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }

        #endregion

        /// <summary>
        /// Static constructor
        /// </summary>
        static DatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DatePicker), new FrameworkPropertyMetadata(typeof(DatePicker)
                    ));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DatePicker()
        {
            Background = Brushes.White;
            currentlyViewedMonth =  DateTime.Now.Month;
            currentlyViewedYear = DateTime.Now.Year;
        }

        
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Event raised when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="e">The arguments to pass</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region Overrides of control
        
        /// <summary>
        /// override to get the templated controls
        /// </summary>
        public override void OnApplyTemplate()
        {
            //Focus the popup if it exists in the Control template
            Popup popup = GetTemplateChild("Popup") as Popup;
            if (popup != null)
            {
                popup.Opened += delegate
                {
                    popup.Focus();
                };
            }

            datesList = GetTemplateChild("PART_Dates") as Selector;
            backButton = GetTemplateChild("PART_MonthBack") as ButtonBase;
            forwardButton = GetTemplateChild("PART_MonthForward") as ButtonBase;
        
            backButton.Click += BackButtonClick;
            forwardButton.Click += ForwardButtonClick;
            datesList.SelectionChanged += DatesListSelectionChanged;
            //if the control is a listbox then set the selection mode
            ListBox list = datesList as ListBox;
            if (list != null && DatesSelectionMode != SelectionMode.Single)
            {
                CurrentlySelectedDates = new ObservableCollection<DateTime>();
                list.SelectionMode = DatesSelectionMode;
            }
            
            ReBindListOfDays();
        }

        //on selected item cahnge of the selector control
        void DatesListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = datesList as ListBox;
            if (list != null 
                && DatesSelectionMode != SelectionMode.Single
                && list.SelectedItems != null)// if the listbox supports multi select
            {
                CurrentlySelectedDates.Clear();
                foreach (object item in list.SelectedItems)
                    CurrentlySelectedDates.Add(GetDateFromCell((DayCell)item));
            }
            else
            {
                if (datesList.SelectedIndex != -1)
                    CurrentlySelectedDate = GetDateFromCell((DayCell)datesList.SelectedItem);
            }
        }

        DateTime GetDateFromCell(DayCell cell)
        {
            return new DateTime(cell.YearNumber, cell.MonthNumber, cell.DayNumber);
        }

        //rebinds to the dates
        void ReBindListOfDays()
        {
            if (datesList != null)
            {
                //Please note that the DateHelper.GetDaysOfMonth gets the days from a cache so it will not have a performance hit to call it everytime
                int numberOfDaysFromPreviousMonth =
                    (int)DateHelper.GetDayOfWeek(currentlyViewedYear, currentlyViewedMonth, 1);

                DayCell[] newDaysTemp = dateHelper.GetDaysOfMonth(currentlyViewedMonth, currentlyViewedYear, MinDate, MaxDate);
                //get the last day of the month and determine the number of days to show from next month
                int numberOfDaysFromNextMonth = 6 - (int)DateHelper.GetDayOfWeek(currentlyViewedYear, currentlyViewedMonth, newDaysTemp[newDaysTemp.Length - 1].DayNumber);
                DayCell[] newDays = new DayCell[newDaysTemp.Length + numberOfDaysFromNextMonth];
                int monthToGetNext;
                int yearTogetNext;
                //get the next month
                DateHelper.MoveMonthForward(currentlyViewedMonth, currentlyViewedYear, out monthToGetNext, out yearTogetNext);
                //get the data for next month
                DayCell[] nextDays = dateHelper.GetDaysOfMonth(monthToGetNext, yearTogetNext, MinDate, MaxDate);//get the next month
                newDaysTemp.CopyTo(newDays, 0);//copy the new days array
                Array.Copy(nextDays, 0, newDays, newDaysTemp.Length, newDays.Length - newDaysTemp.Length);

                DayCell[] listOfDays = new DayCell[numberOfDaysFromPreviousMonth + newDays.Length];
                int monthToGetPrevious;
                int yearTogetPrevious;
                //move one month back
                DateHelper.MoveMonthBack(currentlyViewedMonth, currentlyViewedYear, out monthToGetPrevious, out yearTogetPrevious);
                DayCell[] oldDays = dateHelper.GetDaysOfMonth(monthToGetPrevious, yearTogetPrevious, MinDate, MaxDate);//get the previous month
                Array.Copy(oldDays, oldDays.Length - numberOfDaysFromPreviousMonth, listOfDays, 0, numberOfDaysFromPreviousMonth);
                Array.Copy(newDays, 0, listOfDays, numberOfDaysFromPreviousMonth, newDays.Length);

                //set the item source to the days to show
                datesList.ItemsSource = listOfDays;

                foreach (var day in listOfDays)
                {
                    if (day.YearNumber == CurrentlySelectedDate.Year &&
                        day.MonthNumber == CurrentlySelectedDate.Month &&
                        day.DayNumber == CurrentlySelectedDate.Day)
                    {
                        datesList.SelectedItem = day;
                    }
                }
            }
        }

        //moves the month currently being viewed backward
        void BackButtonClick(object sender, RoutedEventArgs e)
        {
            //move the month back
            DateHelper.MoveMonthBack(currentlyViewedMonth, currentlyViewedYear, out currentlyViewedMonth, out currentlyViewedYear);
            ChangeDate();
        }

        //changes the current date
        void ChangeDate()
        {
            ReBindListOfDays();

            OnPropertyChanged(new PropertyChangedEventArgs(CurrentlyViewedYearPropertyName));
            OnPropertyChanged(new PropertyChangedEventArgs(CurrentlyViewedMonthPropertyName));
            OnPropertyChanged(new PropertyChangedEventArgs(CurrentlyViewedMonthYearPropertyName));
        }

        private void OnDateChanged(DateTime newDate, DateTime oldDate)
        {
            DateSelectedChangedRoutedEventArgs args = 
                new DateSelectedChangedRoutedEventArgs(SelectedDateChangedEvent);
            args.NewDate = newDate;
            args.OldDate = oldDate;

            currentlyViewedMonth = newDate.Month;
            currentlyViewedYear = newDate.Year;
            
            RaiseEvent(args); ;
        }

        //moves the month currently view forward
        void ForwardButtonClick(object sender, RoutedEventArgs e)
        {
            DateHelper.MoveMonthForward(currentlyViewedMonth, currentlyViewedYear, out currentlyViewedMonth, out currentlyViewedYear);
            ChangeDate();
        }

        #endregion
    }

    /// <summary>
    /// Object to represent a single day as a cell
    /// </summary>
    public class DayCell: INotifyPropertyChanged
    {
        readonly int dayNumber;
        /// <summary>
        /// gets the day number for the cell
        /// </summary>
        public int DayNumber 
        {
            get { return dayNumber; } 
        }

        readonly int monthNumber;
        /// <summary>
        /// gets the month number for the cell
        /// </summary>
        public int MonthNumber
        {
            get { return monthNumber; }
        }

        readonly int yearNumber;
        /// <summary>
        /// gets the year number for the cell
        /// </summary>
        public int YearNumber
        {
            get { return yearNumber; }
        }

        bool isEnabled = true;
        /// <summary>
        /// Gets or sets the Enabled status
        /// </summary>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set 
            { 
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="day">The day to store</param>
        /// <param name="month">The month to store</param>
        /// <param name="year">The year to store</param>
        public DayCell(int day, int month, int year)
        {
            dayNumber = day;
            monthNumber = month;
            yearNumber = year;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Property changed event for the enabled status
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }

    #region RoutedEvent
    /// <summary>
    /// Routed event args for the DateSelectedChanged
    /// </summary>
    public class DateSelectedChangedRoutedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Constructor for the event args
        /// </summary>
        /// <param name="routedEvent">The event for which the args will be passed</param>
        public DateSelectedChangedRoutedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent) { }

        /// <summary>
        /// Gets or sets the new date that was set
        /// </summary>
        public DateTime NewDate { get; set; }

        /// <summary>
        /// Gets or sets the old date that was set
        /// </summary>
        public DateTime OldDate { get; set; }
    }

    /// <summary>
    /// Delegate for the DateSelectedChanged event
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="e">Event arguments for the DateSelectedChanged event</param>
    public delegate void DateSelectedChangedEventHandler(object sender, DateSelectedChangedRoutedEventArgs e);
    #endregion

    #region Converters
    /// <summary>
    /// Converter used to compare 2 months
    /// </summary>
    public class IsCurrentMonthConverter : OneWayMultiValueConverter
    {
        /// <summary>
        /// Compares 2 months together
        /// </summary>
        /// <param name="values">The currently view month and the other month to check</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">The parameter to use</param>
        /// <param name="culture">The current culture in use</param>
        /// <returns>Returns true if there is a match between the 2 months</returns>
        public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == null)
                return false;

            int currentlyViewedMonth = (int)values[0];
            int otherMonth = (int)values[1];

            return currentlyViewedMonth == otherMonth;
        }
    }


    /// <summary>
    /// converter to calculate the size for the cell of the calender
    /// </summary>
    public class CellSizeConverter : OneWayValueConverter
    {
        const int daysToFitHorizontal = 7;
        const double minimumValue = 10;//the minum size to return

        #region IValueConverter Members

        /// <summary>
        /// Converter for the calender control to measure the widths to calculate
        /// </summary>
        /// <param name="value">Pass the Actual width of the parent control</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Pass widthCell to calculate the width a particular cell.
        /// Pass widthCellContainer to calculate the witdth of the parent control</param>
        /// <param name="culture">The current culture in use</param>
        /// <returns>Returns the new width to use</returns>
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double valuePassed = (double)value;

            if (parameter != null && !Double.IsNaN(valuePassed))
            {
                if (parameter.ToString() == "widthCell")
                {
                    return Math.Max(valuePassed / daysToFitHorizontal, minimumValue) - 2;
                }

                if (parameter.ToString() == "widthCellContainer")
                {
                    return Math.Max(valuePassed-10, minimumValue);
                }
            }
            return 20.0;
        }

        #endregion
    }

    /// <summary>
    /// converts the date string to the value
    /// </summary>
    public class BoolVisibilityConverter : OneWayValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts the month from a number to the actual string
        /// </summary>
        /// <param name="value">The value as integer</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">The parameter to use</param>
        /// <param name="culture">The current culture in use</param>
        /// <returns>Returns the selected item to select for the drop down list</returns>
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isVisible = (bool)value;

            if (isVisible)
            {
                return Visibility.Visible;
            }

            else
            {
                return Visibility.Collapsed;
            }
        }

        #endregion
    }

    /// <summary>
    /// converts the date string to the value
    /// </summary>
    public class MonthConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts the month from a number to the actual string
        /// </summary>
        /// <param name="value">The value as integer</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">The parameter to use</param>
        /// <param name="culture">The current culture in use</param>
        /// <returns>Returns the selected item to select for the drop down list</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return MonthsDefines.GetMonthString((int)value);
        }

        /// <summary>
        /// Converts the value back from ComboBoxitem to a number that can be set for the current month
        /// </summary>
        /// <param name="value">The comboBoxItem Selected</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">The parameter to use</param>
        /// <param name="culture">The current culture in use</param>
        /// <returns>Returns a number that represents the month selected</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return MonthsDefines.GetMonthNumber((string)value);
        }

        #endregion
    }

    /// <summary>
    /// Validation rule for the Year
    /// </summary>
    public class YearValidation : ValidationRule
    {
        /// <summary>
        /// Validation for the year
        /// </summary>
        /// <param name="value">The year value</param>
        /// <param name="cultureInfo">The culture info</param>
        /// <returns>Returns the validation result</returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int year;
            if (int.TryParse((string)value, out year))
                if (year > 0 && year <= 9999)
                    return new ValidationResult(true, null);
            return new ValidationResult(false, null);
        }
    }
    #endregion
}
