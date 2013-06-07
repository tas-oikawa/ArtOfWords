using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Reflection;

namespace AC.AvalonControlsLibrary.Core
{
    /// <summary>
    /// Base class for comparers that need to be used for the sorting of a control
    /// </summary>
    public abstract class GridViewCustomComparer : DependencyObject, IComparer
    {
        private bool passFullObjectForComparison = false;

        /// <summary>
        /// Set to true if you want the CompareOverride method to pass the full object to compare
        /// Pass false if you want to pass the value of the property to compare
        /// </summary>
        protected bool PassFullObjectForComparison
        {
            get { return passFullObjectForComparison; }
            set { passFullObjectForComparison = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="passFullObjectForComparison">Set to true if you want the CompareOverride method to pass the full object to compare
        /// Pass false if you want to pass the value of the property to compare</param>
        protected GridViewCustomComparer(bool passFullObjectForComparison)
        {
            this.passFullObjectForComparison = passFullObjectForComparison;
        }

        private ListSortDirection direction;

        /// <summary>
        /// Gets or set the direction for sorting
        /// </summary>
        public ListSortDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private string sortPropertyName;

        /// <summary>
        /// The name of the property to sort
        /// </summary>
        public string SortPropertyName
        {
            get { return sortPropertyName; }
            set
            {
                sortPropertyName = value;
            }
        }

        #region IComparer Members

        /// <summary>
        /// The comparison for the class
        /// </summary>
        /// <param name="x">The object value 1</param>
        /// <param name="y">The object value 2</param>
        /// <returns>Returns a number taht determines the order to sort</returns>
        public int Compare(object x, object y)
        {
            if (passFullObjectForComparison)
                return CompareOverride(x, y);

            //get the actual values of the property to compare
            BindingOperations.ClearAllBindings(this);

            Binding bindForX = new Binding(SortPropertyName);
            bindForX.Source = x;
            BindingOperations.SetBinding(this, ValueForXProperty, bindForX);

            Binding bindForY = new Binding(SortPropertyName);
            bindForY.Source = y;
            BindingOperations.SetBinding(this, ValueForYProperty, bindForY);

            object valueX = this.GetValue(ValueForXProperty);
            object valueY = this.GetValue(ValueForYProperty);

            return CompareOverride(valueX, valueY);
        }

        //dependency property used for getting the value of the object x
        private static readonly DependencyProperty ValueForXProperty =
            DependencyProperty.Register("ValueForX", typeof(object), typeof(GridViewCustomComparer));

        //dependency property used for getting the value of the object y
        private static readonly DependencyProperty ValueForYProperty =
            DependencyProperty.Register("ValueForY", typeof(object), typeof(GridViewCustomComparer));

        /// <summary>
        /// The comparison for the class
        /// </summary>
        /// <param name="x">The object value 1</param>
        /// <param name="y">The object value 2</param>
        /// <returns>Returns a number taht determines the order to sort</returns>
        public abstract int CompareOverride(object x, object y);

        #endregion
    }

    /// <summary>
    /// Default comparer that can only work with properties that implement the IComparable interface or the IComparer interface
    /// </summary>
    public class DefaultListViewComparer : GridViewCustomComparer
    {
        /// <summary>
        /// Pass true when using the comparer with simple binding 
        /// Pass false when using complex binding for properties (ex MyProp[test])
        /// </summary>
        public bool UseSimpleBinding
        {
            get { return base.PassFullObjectForComparison; }
            set { base.PassFullObjectForComparison = value; }
        }


        /// <summary>
        /// default constructor
        /// </summary>
        public DefaultListViewComparer() : base(false) { }

        /// <summary>
        /// The comparison for the class
        /// </summary>
        /// <param name="x">The object value 1</param>
        /// <param name="y">The object value 2</param>
        /// <returns>Returns a number taht determines the order to sort</returns>
        /// <exception cref="ArgumentException">Raised when one of the properties to compare does not implement the IComparer or the IComparable</exception>
        public override int CompareOverride(object x, object y)
        {
            if (x == null || y == null)
                return 0;

            if (UseSimpleBinding)
            {
                //get property value
                PropertyInfo prop = x.GetType().GetProperty(SortPropertyName);
                x = prop.GetValue(x, null);
                y = prop.GetValue(y, null);
            }

            int result = 0;

            IComparer comparer = x as IComparer;

            if (comparer != null)
            {
                result = comparer.Compare(x, y);
            }
            else
            {
                IComparable comparable = x as IComparable;
                if (comparable != null)
                    result = comparable.CompareTo(y);
                else
                    throw new ArgumentException("Invalid parameters passed. Property must implement IComparer or IComparable",
                        String.Format("Property Name - {0}", SortPropertyName));
            }

            if (Direction == System.ComponentModel.ListSortDirection.Ascending)
                return result;
            else
                return (-1) * result;
        }
    }
}
