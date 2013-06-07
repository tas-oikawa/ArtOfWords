using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using AC.AvalonControlsLibrary.Exception;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// Event args to pass when sending the Item mouse over event
    /// </summary>
    internal class RateItemMouseOverEventArgs : EventArgs
    {
        private int rateValue;
        /// <summary>
        /// Gets or Sets the rate value selected
        /// </summary>
        public int RateValue 
        { 
            get { return rateValue; } 
            set { rateValue = value; } 
        }

        bool cancel;
        /// <summary>
        /// Gets or sets a flag that signals if the items should cancel the mouse over
        /// </summary>
        public bool Cancel 
        {
            get { return cancel; }
            set { cancel = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rateValue">The rate value to pass</param>
        /// <param name="cancel">Pass true to cancel the rate mouse over</param>
        public RateItemMouseOverEventArgs(int rateValue, bool cancel)
        {
            this.rateValue = rateValue;
            this.cancel = cancel;
        }
    }

    /// <summary>
    /// RatingSelector is a control that lets you select a rating
    /// </summary>
    public class RatingSelector : ItemsControl
    {
        //the list of items inside the control
        ObservableCollection<RatingSelectorItem> ratingItems = new ObservableCollection<RatingSelectorItem>();

        #region Properties

        /// <summary>
        /// Event raised to notify all rate items that another rate item has been mouse over
        /// </summary>
        internal event EventHandler<RateItemMouseOverEventArgs> RateItemMouseOver;

        /// <summary>
        /// Gets or sets the color for the hover color of the rate item
        /// </summary>
        public Brush RateItemHoverColor
        {
            get { return (Brush)GetValue(RateItemHoverColorProperty); }
            set { SetValue(RateItemHoverColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color for the hover color of the rate item
        /// </summary>
        public static readonly DependencyProperty RateItemHoverColorProperty =
            DependencyProperty.Register("RateItemHoverColor", typeof(Brush), typeof(RatingSelector), new UIPropertyMetadata(Brushes.YellowGreen));


        /// <summary>
        /// Gets or sets the color to use for the rating control
        /// </summary>
        public Brush RateItemColor
        {
            get { return (Brush)GetValue(RateItemColorProperty); }
            set { SetValue(RateItemColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to use for the rating control
        /// </summary>
        public static readonly DependencyProperty RateItemColorProperty =
            DependencyProperty.Register("RateItemColor", typeof(Brush), typeof(RatingSelector), new UIPropertyMetadata(Brushes.Yellow));

        /// <summary>
        /// Gets or sets a flag that signals if the control should be readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a flag that signals if the control should be readonly
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RatingSelector), new UIPropertyMetadata(false));


        /// <summary>
        /// Gets or sets the rating selected
        /// </summary>
        public int RatingSelected
        {
            get { return (int)GetValue(RatingSelectedProperty); }
            set { SetValue(RatingSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the rating selected
        /// </summary>
        public static readonly DependencyProperty RatingSelectedProperty;

        /// <summary>
        /// Gets and Sets the min rating for the control
        /// </summary>
        public int MinRating
        {
            get { return (int)GetValue(MinRatingProperty); }
            set { SetValue(MinRatingProperty, value); }
        }

        /// <summary>
        /// Gets and Sets the min rating for the control
        /// </summary>
        public static readonly DependencyProperty MinRatingProperty;

        /// <summary>
        /// Gets and Sets the max rating for the control
        /// </summary>
        public int MaxRating
        {
            get { return (int)GetValue(MaxRatingProperty); }
            set { SetValue(MaxRatingProperty, value); }
        }

        /// <summary>
        /// Gets and Sets the max rating for the control
        /// </summary>
        public static readonly DependencyProperty MaxRatingProperty;

        /// <summary>
        /// Command to select rating
        /// </summary>
        public static RoutedUICommand SelectRating = 
            new RoutedUICommand("Select Rating", "SelectRating", typeof(RatingSelector));

        #endregion

        #region property changes
        //handler min rating selected
        static void MinRatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RatingSelector selector = (RatingSelector)sender;
            if (!selector.IsInitialized)
                return;
            int oldValue = (int)e.OldValue;
            int newValue = (int)e.NewValue;
            int diff = newValue - oldValue;
            bool decreaseRange = diff > 0;
            AddRemoveItems(selector, diff, decreaseRange);
            selector.CoerceValue(RatingSelector.MaxRatingProperty);
        }

        //handler max rating selected
        static void MaxRatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RatingSelector selector = (RatingSelector)sender;
            if (!selector.IsInitialized)
                return;
            int oldValue = (int)e.OldValue;
            int newValue = (int)e.NewValue;
            int diff = oldValue - newValue;
            bool decreaseRange = diff > 0;
            AddRemoveItems(selector, diff, decreaseRange);
            selector.CoerceValue(RatingSelector.MinRatingProperty);
        }

        //handler for the property changed of the ratingselected prop
        static void RatingSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RatingSelector selector = (RatingSelector)sender;
            if (!selector.IsInitialized)
                return;
            selector.SelectItems();
        }

        #endregion

        #region Coerce handlers
        //validate the selected rating
        static object ForceRatingSelected(DependencyObject sender, object value)
        {
            RatingSelector selector = (RatingSelector)sender;
            int val = (int)value;
            val = Math.Max(selector.MinRating, val);
            val = Math.Min(selector.MaxRating, val);
            return val;
        }
        //make sure that the value is valid
        static object ForceMaxRating(DependencyObject sender, object value)
        {
            return Math.Max( ((RatingSelector)sender).MinRating, (int)value );            
        }
        //make sure that the value is valid
        static object ForceMinRating(DependencyObject sender, object value)
        {
            return Math.Max(0, Math.Min(((RatingSelector)sender).MaxRating, (int)value));
        }
        #endregion

        /// <summary>
        /// Static constructor
        /// This overrides the default style
        /// </summary>
        static RatingSelector()
        {
            MinRatingProperty =
                DependencyProperty.Register("MinRating", typeof(int),
                typeof(RatingSelector), new UIPropertyMetadata(0, MinRatingChanged, ForceMinRating));

            MaxRatingProperty =
                DependencyProperty.Register("MaxRating", typeof(int),
                typeof(RatingSelector), new UIPropertyMetadata(5, MaxRatingChanged, ForceMaxRating));

            RatingSelectedProperty = DependencyProperty.Register("RatingSelected", typeof(int),
                typeof(RatingSelector), new UIPropertyMetadata(0, RatingSelectedChanged, ForceRatingSelected));

            //override to make the listbox item look like a tree view item
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RatingSelector), new FrameworkPropertyMetadata(typeof(RatingSelector)
                    ));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public RatingSelector()
        {
            //bind the command to the handler
            CommandBindings.Add(new CommandBinding(SelectRating, SelectRatingHandler, CanExecuteSelectRating));

            Initialized += delegate
            {
                //popoulate the items
                PopulateItems();
            };

            //when the mouse leaves the control force the reselection of items
            MouseLeave += delegate
            {
                NotifyItemMouseOver(RatingSelected, true);
            };
        }

        //the command can execute only if the IsReadOnly is false
        void CanExecuteSelectRating(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsReadOnly;
        }

        //handler for the rating
        void SelectRatingHandler(object sender, ExecutedRoutedEventArgs e)
        {
            int selectedRating = (int)e.Parameter;
            RatingSelected = selectedRating;
        }

        //raises the RateItemMouseOver event so that all children handle it
        internal void NotifyItemMouseOver(int rateValue, bool cancel)
        {
            //cancel only if the the mouse is not over the control
            if (cancel && IsMouseOver)
                return;

            if (RateItemMouseOver != null)
                RateItemMouseOver(this, new RateItemMouseOverEventArgs(rateValue, cancel));
        }
        
        #region overrides for the itemscontrol
        /// <summary>
        /// override in order to make sure that only RatingSelectorItems are populated
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (!(item is RatingSelectorItem))
                throw new NotSupportedException(ExceptionStrings.NOT_SUPPORTED_RATINGITEM);
            return true;
        }
        /// <summary>
        /// create an instance of the rating item
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            throw new NotSupportedException(ExceptionStrings.NOT_SUPPORTED_RATINGITEM);
        }
        #endregion

        #region Helper methods
        //populates the items for the rating
        private void PopulateItems()
        {
            int totalItems = MaxRating - MinRating;
            for (int i = 1; i <= totalItems; i++)
                ratingItems.Add(CreateItem(i, i <= RatingSelected));

            //set the items to be the rating items
            ItemsSource = ratingItems;
        }

        //adds or removes items from the list
        private static void AddRemoveItems(RatingSelector selector, int diff, bool decreaseRange)
        {
            if (decreaseRange)
            {
                //remove items that are not needed
                for (int i = 0; i < diff; i++)
                {
                    //check if there are any items to delete
                    if (selector.ratingItems.Count == 0)
                        break;
                    selector.ratingItems.RemoveAt(0);
                }
            }
            else
            {
                diff = -diff;//since this would be a negative number
                for (int i = 0; i < diff; i++)
                    selector.ratingItems.Insert(0, selector.CreateItem(i, false));
            }

            //re arrange items
            selector.ReAssignRateValue();
            selector.SelectItems();
        }
        
        private void ReAssignRateValue()
        {
            int counter = 1;
            foreach (RatingSelectorItem item in ratingItems)
            {
                item.RateValue = counter;
                counter++;
            }
        }

        //creates an rating item 
        private RatingSelectorItem CreateItem(int itemValue, bool selected)
        {
            RatingSelectorItem newItem = new RatingSelectorItem(this);
            newItem.Content = itemValue;
            newItem.RateValue = itemValue;
            newItem.IsSelected = selected;
            return newItem;
        }

        //Select the items in the list according to the selected Rate
        private void SelectItems()
        {
            foreach (RatingSelectorItem item in ratingItems)
                item.IsSelected = item.RateValue <= RatingSelected;
        }
        #endregion
    }

    /// <summary>
    /// Items for the rating seletor
    /// </summary>
    public class RatingSelectorItem : ContentControl
    {
        #region Properties
        /// <summary>
        /// Gets a value indicating that a higher rate than the current has the mouse over
        /// </summary>
        public bool IsHigherRateMouseOver
        {
            get { return (bool)GetValue(IsHigherRateMouseOverProperty); }
            set { SetValue(IsHigherRateMouseOverProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating that a higher rate than the current has the mouse over
        /// </summary>
        public static readonly DependencyProperty IsHigherRateMouseOverProperty =
            DependencyProperty.Register("IsHigherRateMouseOver", typeof(bool), typeof(RatingSelectorItem), new UIPropertyMetadata(false));


        /// <summary>
        /// The rating value for the item
        /// </summary>
        public int RateValue
        {
            get { return (int)GetValue(RateValueProperty); }
            internal set { SetValue(RateValueProperty, value); }
        }

        /// <summary>
        /// The rating value for the item
        /// </summary>
        public static readonly DependencyProperty RateValueProperty =
            DependencyProperty.Register("RateValue", typeof(int), 
            typeof(RatingSelectorItem), new UIPropertyMetadata(0));

        /// <summary>
        /// gets or sets if the item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// gets or sets if the item is selected
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(RatingSelectorItem), new UIPropertyMetadata(false));

        #endregion

        /// <summary>
        /// Static constructor
        /// This overrides the default style
        /// </summary>
        static RatingSelectorItem()
        {
            //override to make the listbox item look like a tree view item
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(RatingSelectorItem), new FrameworkPropertyMetadata(typeof(RatingSelectorItem)
                    ));
        }

        RatingSelector parentSelector;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parentSelector">The owner of the item</param>
        public RatingSelectorItem(RatingSelector parentSelector)
        {
            this.parentSelector = parentSelector;
            //handle the parentSelector
            this.parentSelector.RateItemMouseOver += new EventHandler<RateItemMouseOverEventArgs>(ParentSelectorRateItemMouseOver);
            
            //when the item is mouse over raise the event so that the other rate items know about it
            MouseEnter += delegate
            {
                parentSelector.NotifyItemMouseOver(RateValue, false);
            };

            MouseLeave += delegate
            {
                parentSelector.NotifyItemMouseOver(RateValue, true);
            };
        }

        //handler for the rate item mouse over
        void ParentSelectorRateItemMouseOver(object sender, RateItemMouseOverEventArgs args)
        {
            if (args.Cancel)
                IsHigherRateMouseOver = false;
            else
                IsHigherRateMouseOver = args.RateValue >= RateValue;
        }
    }
}
