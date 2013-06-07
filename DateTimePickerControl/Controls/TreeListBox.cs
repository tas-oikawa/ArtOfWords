using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using AC.AvalonControlsLibrary.Core;
using System.Globalization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// VirtualizingTreeView is a listbox that can show a hierarchy of objects just like a tree view. 
    /// This control uses UI virtualization
    /// </summary>
    public class TreeListBox : ListBox
    {
        #region Events
        /// <summary>
        /// Event raised when new items are created
        /// </summary>
        public event EventHandler<TreeListBoxItemCreatedEventArgs> NewItemCreated;

        /// <summary>
        /// raises the NewItemCreated
        /// </summary>
        /// <param name="e">The event arguments for the event</param>
        protected void OnNewItemCreated(TreeListBoxItemCreatedEventArgs e)
        {
            if (NewItemCreated != null)
                NewItemCreated(this, e);
        }
        #endregion

        #region Properties

        //stores the child list of VirtualizingTreeViewInfo
        private List<TreeListBoxInfo> rootNodesInfo;

        /// <summary>
        /// The property name of the property that exposes the child collection of items
        /// </summary>
        public string ChildItemSourcePath
        {
            get { return (string)GetValue(ChildItemSourcePathProperty); }
            set { SetValue(ChildItemSourcePathProperty, value); }
        }

        /// <summary>
        /// The property name of the property that exposes the child collection of items
        /// </summary>
        public static readonly DependencyProperty ChildItemSourcePathProperty =
            DependencyProperty.Register("ChildItemSourcePath", typeof(string), typeof(TreeListBox),
            new UIPropertyMetadata(null));

        //stores the flattened list of data. 
        //The data is stored in an object of type VirtualizingTreeViewInfo so that it can store info such as level and children count
        private ObservableCollection<TreeListBoxInfo> compositeChildCollection =
            new ObservableCollection<TreeListBoxInfo>();

        /// <summary>
        /// Gets or sets the composite collection that contains all child elements
        /// </summary>
        internal ObservableCollection<TreeListBoxInfo> CompositeChildCollection
        {
            get { return compositeChildCollection; }
        }

        /// <summary>
        /// Gets or sets the items source for the hierarchal data items
        /// </summary>
        public IEnumerable HierarchalItemsSource
        {
            get { return (IEnumerable)GetValue(HierarchalItemsSourceProperty); }
            set { SetValue(HierarchalItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the items source for the hierarchal data items
        /// </summary>
        public static readonly DependencyProperty HierarchalItemsSourceProperty =
            DependencyProperty.Register("HierarchalItemsSource", typeof(IEnumerable),
            typeof(TreeListBox), new UIPropertyMetadata(null,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    ((TreeListBox)sender).GenerateItemsSource(e.OldValue as IEnumerable, e.NewValue as IEnumerable);
                }));
        #endregion

        #region Generation of flattened list
        //event handler for itemssource notifications
        NotifyCollectionChangedEventHandler notificationEnableCollectionHandler;

        //Generates the items source for the list
        private void GenerateItemsSource(IEnumerable oldValue, IEnumerable newValue)
        {
            ResetItemsSource(newValue);

            //register and unregister to notifications of the collection
            if (notificationEnableCollectionHandler == null)
                notificationEnableCollectionHandler = new NotifyCollectionChangedEventHandler(NotificationEnableCollectionCollectionChanged);

            INotifyCollectionChanged oldNotificationEnableCollection = oldValue as INotifyCollectionChanged;
            if (oldNotificationEnableCollection != null)
                oldNotificationEnableCollection.CollectionChanged -= notificationEnableCollectionHandler;

            INotifyCollectionChanged newNotificationEnableCollection = newValue as INotifyCollectionChanged;
            if (newNotificationEnableCollection != null)
                newNotificationEnableCollection.CollectionChanged += notificationEnableCollectionHandler;
        }

        //resets the items source
        private void ResetItemsSource(IEnumerable newValue)
        {
            if (newValue != null)
            {
                compositeChildCollection.Clear();

                if (rootNodesInfo == null)
                    rootNodesInfo = new List<TreeListBoxInfo>();
                else
                    rootNodesInfo.Clear();

                foreach (object item in newValue)
                {
                    TreeListBoxInfo info = new TreeListBoxInfo(0, item);
                    //add the new info to the main flattened list
                    compositeChildCollection.Add(info);
                    // add the new info to intenal list of data items
                    rootNodesInfo.Add(info);
                }
                //set the items sources to be out compositeChildCollection that contains all the VirtualizingTreeViewInfo
                ItemsSource = compositeChildCollection;
            }
            else
                ItemsSource = null;// if the new source is null set the source to be null
        }

        //event handler for the collection change of the items source
        void NotificationEnableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        int counter = 0;
                        foreach (object item in e.NewItems)
                        {
                            //get the destination index
                            int originalDestinationIndex =
                                Math.Min(e.NewStartingIndex + counter, rootNodesInfo.Count);

                            //walk through the list of ancestors and count their children
                            int newDestinationIndex = 0;
                            for (int i = originalDestinationIndex - 1; i >= 0; i--)
                                newDestinationIndex += (rootNodesInfo[i].ChildrenCount + 1);

                            originalDestinationIndex = newDestinationIndex;//if the destination is 0 you still need to add 1

                            //create the new list item
                            TreeListBoxInfo newInfo = new TreeListBoxInfo(0, item);
                            //add the new item in the internal list
                            rootNodesInfo.Insert(Math.Min(e.NewStartingIndex + counter, rootNodesInfo.Count), newInfo);
                            //add to the main list(the flattened list)
                            compositeChildCollection.Insert(originalDestinationIndex, newInfo);
                            counter++;
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (compositeChildCollection.Count > e.OldStartingIndex)
                    {
                        //remove all children
                        TreeListBoxInfo info = rootNodesInfo[e.OldStartingIndex];
                        //remove all child items
                        for (int i = 0; i < info.ChildrenCount; i++)
                            compositeChildCollection.RemoveAt(e.OldStartingIndex);
                        //remove the actual item
                        compositeChildCollection.RemoveAt(e.OldStartingIndex);
                        rootNodesInfo.RemoveAt(e.OldStartingIndex);
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    //reset the items source
                    ResetItemsSource(sender as IEnumerable);
                    break;
            }
        }
        #endregion

        #region Listbox overrides
        /// <summary>
        /// Verifies if the item passed is a VirtualizingTreeViewItem
        /// </summary>
        /// <param name="item">The item to verify</param>
        /// <returns>Returns true if the item is a VirtualizingTreeViewItem</returns>
        /// <exception cref="NotSupportedException">The current implementation does not suupport generation of it's own kind</exception>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            if (item is TreeListBoxItem)
                throw new NotSupportedException("The current implementation does not support generation of it's own kind");
            else
                return false;
        }

        /// <summary>
        /// Creates an instance of the VirtualizingTreeViewItem
        /// </summary>
        /// <returns>Returns the instance of the new VirtualizingTreeViewItem</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListBoxItem(this);
        }

        /// <summary>
        /// Prepares the new VirtualizingTreeViewItem with the actual business object
        /// </summary>
        /// <param name="element">The element (VirtualizingTreeViewItem) to apply the template</param>
        /// <param name="item">The business object to set</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            TreeListBoxItem listItem = element as TreeListBoxItem;
            TreeListBoxInfo actualItem = item as TreeListBoxInfo;

            //raise the event to signal that a new item is created
            OnNewItemCreated(new TreeListBoxItemCreatedEventArgs(listItem, actualItem));
            //prepares the item with the relative VirtualizingTreeViewInfo
            listItem.PrepareItem(actualItem);
            //pass the actual data item instead of the VirtualizingTreeViewInfo
            base.PrepareContainerForItemOverride(element, actualItem.DataItem);
        }

        /// <summary>
        /// override the Items source changed to verify that the items source set was set from the control itself
        /// </summary>
        /// <param name="oldValue">The old items source</param>
        /// <param name="newValue">The new items source</param>
        /// <exception cref="ArgumentException">Thrown when the user sets the Items source property</exception>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (!(newValue is ObservableCollection<TreeListBoxInfo>))
                throw new ArgumentException("Do not use the ItemsSource property for this control please use the HierarchalItemsSource", "newValue");
            base.OnItemsSourceChanged(oldValue, newValue);
        }

        #endregion

        #region Unit Tests
        /// <summary>
        /// Exposes the ResetItemsSource
        /// </summary>
        /// <param name="newValue">A list containing buisness objects</param>
        [System.Diagnostics.Conditional("UNIT_TESTS")]
        public void ExposedGenerateItemsSource(IEnumerable newValue)
        {
            GenerateItemsSource(null, newValue);
        }

        #endregion
    }

    /// <summary>
    /// Holds the info for a specific VirtualizingTreeViewItem
    /// </summary>
    public class TreeListBoxInfo
    {
        #region Properties

        private int level;

        /// <summary>
        /// Gets or sets the level for the current VirtualizingTreeViewItem
        /// </summary>
        public int Level
        {
            get { return level; }
        }

        /// <summary>
        /// Gets the child count of the current item.
        /// This includes the children at all levels
        /// </summary>
        public int ChildrenCount
        {
            get
            {
                return GetChildCount(this);
            }
        }

        //gets the child count by walking the children tree
        private static int GetChildCount(TreeListBoxInfo info)
        {
            if (info.childItems == null || info.childItems.Count == 0)
                return 0;

            int childCount = 0;
            foreach (TreeListBoxInfo child in info.childItems)
            {
                childCount += GetChildCount(child); // add the child count
                childCount++;//add the current item
            }
            return childCount;
        }

        private object dataItem;

        /// <summary>
        /// Gets the business object of the current item
        /// </summary>
        public object DataItem
        {
            get { return dataItem; }
        }

        private bool isExpanded;
        /// <summary>
        /// Gets or sets the state of the item expansion
        /// </summary>
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { isExpanded = value; }
        }

        //stores an internal list of the child TreeListBoxInfo
        private List<TreeListBoxInfo> childItems;

        /// <summary>
        /// return the child TreeListBoxInfo objects
        /// </summary>
        public List<TreeListBoxInfo> ChildItems
        {
            get
            {
                if (childItems == null)
                    childItems = new List<TreeListBoxInfo>();
                return childItems;
            }
        }
        #endregion

        /// <summary>
        /// Constructor 
        /// Sets the default values for the data members
        /// </summary>
        /// <param name="level">The level of the item</param>
        /// <param name="dataItem"><see cref="DataItem"/>The actual data item</param>
        public TreeListBoxInfo(int level, object dataItem)
        {
            this.level = level;
            this.dataItem = dataItem;
        }
    }

    /// <summary>
    /// event args passed when a tree list box item change
    /// </summary>
    public class TreeListBoxItemCreatedEventArgs : EventArgs
    {
        private TreeListBoxItem newItem;
        private TreeListBoxInfo newItemInfo;
        /// <summary>
        /// gets the new item created
        /// </summary>
        public TreeListBoxItem NewItem
        {
            get { return newItem; }
        }

        /// <summary>
        /// gets the new item info for the newly created item
        /// </summary>
        public TreeListBoxInfo NewItemInfo
        {
            get { return newItemInfo; }
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="item">The new item created</param>
        /// <param name="info">The info relative to this item</param>
        public TreeListBoxItemCreatedEventArgs(TreeListBoxItem item, TreeListBoxInfo info)
        {
            newItem = item;
            newItemInfo = info;
        }
    }
}
