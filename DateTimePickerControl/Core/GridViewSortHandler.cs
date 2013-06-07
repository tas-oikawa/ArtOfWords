using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AC.AvalonControlsLibrary.Controls;
using System.ComponentModel;
using System.Collections;
using System.Windows.Data;

namespace AC.AvalonControlsLibrary.Core
{
    /// <summary>
    /// Handles sorting on any type of Listvew or Listview derived types
    /// </summary>
    public class GridViewSortHandler
    {
        #region Attached Properties
        /// <summary>
        /// Attached property to make a specific listview support sorting
        /// </summary>
        public readonly static DependencyProperty GridViewSortHandlerProperty = 
            DependencyProperty.RegisterAttached(
            "GridViewSortHandler", typeof(GridViewSortHandler), typeof(GridViewSortHandler),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(RegisterSortHandler)));

        /// <summary>
        /// gets the value GridViewSortable from the object
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>Returns the value of the GridViewSortable property</returns>
        public static GridViewSortHandler GetGridViewSortHandler(DependencyObject obj)
        {
            return (GridViewSortHandler)obj.GetValue(GridViewSortHandlerProperty);
        }

        /// <summary>
        /// Sets the value GridViewSortable on the specified object
        /// </summary>
        /// <param name="obj">The object to store the value of the property</param>
        /// <param name="value">The value to set</param>
        public static void SetGridViewSortHandler(DependencyObject obj, GridViewSortHandler value)
        {
            obj.SetValue(GridViewSortHandlerProperty, value);
        }

        /// <summary>
        /// Attached property to make a specific listview support sorting
        /// </summary>
        public readonly static DependencyProperty CustomComparerProperty =
            DependencyProperty.RegisterAttached(
            "CustomComparer", typeof(GridViewCustomComparer), typeof(GridViewSortHandler),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// gets the value GridViewCustomComparer from the object
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>Returns the value of the GridViewSortable property</returns>
        public static GridViewCustomComparer GetCustomComparer(DependencyObject obj)
        {
            return (GridViewCustomComparer)obj.GetValue(CustomComparerProperty);
        }

        /// <summary>
        /// Sets the value GridViewCustomComparer on the specified object
        /// </summary>
        /// <param name="obj">The object to store the value of the property</param>
        /// <param name="value">The value to set</param>
        public static void SetCustomComparer(DependencyObject obj, GridViewCustomComparer value)
        {
            obj.SetValue(CustomComparerProperty, value);
        }

        #endregion

        #region Handler properties
        
        string columnHeaderSortedAscendingTemplate;
        /// <summary>
        /// Template for the column header to sort in asc order
        /// </summary>
        public string ColumnHeaderSortedAscendingTemplate
        {
            get { return columnHeaderSortedAscendingTemplate; }
            set { columnHeaderSortedAscendingTemplate = value; }
        }

        string columnHeaderSortedDescendingTemplate;
        /// <summary>
        /// The template for the column header for sorting in desc order
        /// </summary>
        public string ColumnHeaderSortedDescendingTemplate
        {
            get { return columnHeaderSortedDescendingTemplate; }
            set { columnHeaderSortedDescendingTemplate = value; }
        }

        string columnHeaderNotSortedTemplate;
        /// <summary>
        /// Template for the column header when it is not being sorted
        /// </summary>
        public string ColumnHeaderNotSortedTemplate
        {
            get { return columnHeaderNotSortedTemplate; }
            set { columnHeaderNotSortedTemplate = value; }
        }

        SortableGridViewColumn lastSortedOnColumn;//stores the last column that was sorted
        /// <summary>
        /// Returns the last sorted column
        /// </summary>
        public SortableGridViewColumn LastSortedOnColumn
        {
            get { return lastSortedOnColumn; }
        }

        private GridViewCustomComparer comparer;

        /// <summary>
        /// The comparer to use for the sorting
        /// </summary>
        public GridViewCustomComparer Comparer
        {
            get{ return comparer; }
            set { comparer = value; }
        }

        private ListView parentControl;

        /// <summary>
        /// gets the parent control for the sort handler
        /// this is set by the RegisterSortHandler
        /// </summary>
        public ListView ParentControl
        {
            get { return parentControl; }
        }
        #endregion

        //event handler for the click event
        static RoutedEventHandler columnClickhandler = new RoutedEventHandler(OnColumnHeaderClicked);

        //registers to the column click of the header to apply the sort
        private static void RegisterSortHandler(DependencyObject obj,
          DependencyPropertyChangedEventArgs args)
        {
            ListView grid = obj as ListView;
            if (grid == null)
                return;
         
            GridViewSortHandler handler = args.NewValue as GridViewSortHandler;
            if(handler != null)
            {
                //register to click events of column header
                grid.AddHandler(GridViewColumnHeader.ClickEvent,
                    columnClickhandler);
            }
        }

        //event handler for the GridViewColumnHeader click
        private static void OnColumnHeaderClicked(object sender, RoutedEventArgs e)
        {
            ListView view = sender as ListView;
            if (view == null)
                return;

            GridViewSortHandler handler = view.GetValue(GridViewSortHandlerProperty) as GridViewSortHandler;
            if (handler == null)
                return;
            
            //set the parent control of the handler
            handler.parentControl = view;
            handler.comparer = view.GetValue(CustomComparerProperty) as GridViewCustomComparer;

            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            // ensure that we clicked on the column header and not the padding that's added to fill the space.
            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                // attempt to cast to the sortableGridViewColumn object.
                SortableGridViewColumn sortableGridViewColumn = headerClicked.Column as SortableGridViewColumn;
                if(sortableGridViewColumn.CanSort)
                    handler.ApplySort(sortableGridViewColumn);
            }
        }

        #region Sorting
        /// <summary>
        /// sorts the grid view by a specified column
        /// </summary>
        /// <param name="sortableGridViewColumn">The column to sort</param>
        public void ApplySort(SortableGridViewColumn sortableGridViewColumn)
        {
            // ensure that the column header is the correct type and a sort property has been set.
            if (sortableGridViewColumn != null && !String.IsNullOrEmpty(sortableGridViewColumn.SortPropertyName))
            {
                if (Comparer == null) //default sort
                {
                    //get the default collevtion view
                    parentControl.Items.SortDescriptions.Clear();
                    SortDescription sd = new SortDescription(
                        sortableGridViewColumn.SortPropertyName, sortableGridViewColumn.SortDirection);
                    parentControl.Items.SortDescriptions.Add(sd);
                }
                else
                {
                    //get a collection view
                    ListCollectionView defaultCollectionView = 
                        CollectionViewSource.GetDefaultView(parentControl.ItemsSource) as ListCollectionView;
                    if (defaultCollectionView != null)
                    {
                        Comparer.Direction = sortableGridViewColumn.SortDirection;
                        Comparer.SortPropertyName = sortableGridViewColumn.SortPropertyName;
                        defaultCollectionView.CustomSort = comparer;
                    }
                }

                sortableGridViewColumn.SetSortDirection();//reset the direction

                bool newSortColumn = false;
                // determine if this is a new sort
                if (lastSortedOnColumn == null
                    || String.IsNullOrEmpty(lastSortedOnColumn.SortPropertyName)
                    || !String.Equals(sortableGridViewColumn.SortPropertyName, lastSortedOnColumn.SortPropertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    newSortColumn = true;
                }

                //apply the templates
                if (sortableGridViewColumn.SortDirection == ListSortDirection.Ascending)
                {
                    if (!String.IsNullOrEmpty(this.ColumnHeaderSortedAscendingTemplate))
                        sortableGridViewColumn.HeaderTemplate = parentControl.TryFindResource(ColumnHeaderSortedAscendingTemplate) as DataTemplate;
                    else
                        sortableGridViewColumn.HeaderTemplate = null;
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.ColumnHeaderSortedDescendingTemplate))
                        sortableGridViewColumn.HeaderTemplate = parentControl.TryFindResource(ColumnHeaderSortedDescendingTemplate) as DataTemplate;
                    else
                        sortableGridViewColumn.HeaderTemplate = null;

                }

                // Remove arrow from previously sorted header
                if (newSortColumn && lastSortedOnColumn != null)
                {
                    if (!String.IsNullOrEmpty(this.ColumnHeaderNotSortedTemplate))
                        lastSortedOnColumn.HeaderTemplate = parentControl.TryFindResource(ColumnHeaderNotSortedTemplate) as DataTemplate;
                    else
                        lastSortedOnColumn.HeaderTemplate = null;
                }

                lastSortedOnColumn = sortableGridViewColumn;
            }
        }
        #endregion
    }
}
