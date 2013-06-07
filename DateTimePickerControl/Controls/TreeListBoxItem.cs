using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using AC.AvalonControlsLibrary.Core;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// The UI container that represents an item for the TreeListBox
    /// </summary>
    [TemplatePart(Name = "PART_Header", Type = typeof(ToggleButton))]
    public class TreeListBoxItem : ListBoxItem
    {
        #region TreeListBox Properties and Data memebers
        //event handler for collection notification of the children
        private NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler;
        //collection that contains the children
        private INotifyCollectionChanged childrenNotificationList;
        //stores child collection property info
        private PropertyInfo childCollectionProperty;
        //counter of child items created
        private int lastChildItemCreatedIndex = 1;
        //stores the current info for this TreeListBoxItem
        private TreeListBoxInfo currentInfo;
        //stores the parent TreeListBox
        private readonly TreeListBox treeListBox;
        //flag to signal that the item is initializing
        private bool isInitializing = true;

        //stores the current the level number in the hierarchy
        private int level;
        /// <summary>
        /// Gets the current level number in the hierarchy
        /// </summary>
        public int Level
        {
            get { return level; }
        }

        /// <summary>
        /// Returns true if the item is expanded
        /// </summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// Returns true if the item is expanded
        /// </summary>
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeListBoxItem), new UIPropertyMetadata(false,
                delegate(DependencyObject sender, DependencyPropertyChangedEventArgs e)
                {
                    TreeListBoxItem item = (TreeListBoxItem)sender;
                    //check if the property was set while initializing.
                    //this can happen when the item was virtualized and then re created
                    if (!item.isInitializing)
                        item.ExpandCollapseItem();
                }));

        #endregion

        /// <summary>
        /// Static constructor
        /// This overrides the default style
        /// </summary>
        static TreeListBoxItem()
        {
            //override to make the listbox item look like a tree view item
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TreeListBoxItem), new FrameworkPropertyMetadata(typeof(TreeListBoxItem)
                    ));
        }

        /// <summary>
        /// constructor for the TreeListBoxItem
        /// </summary>
        /// <param name="parent">The parent TreeListBox</param>
        public TreeListBoxItem(TreeListBox parent)
        {
            treeListBox = parent;

            Unloaded += delegate
            {
                if (!currentInfo.IsExpanded)
                    UnRegisterCollectionNotification();
            };
        }

        /// <summary>
        /// Prepares the item by setting the properties
        /// Override this method if you want to attach any extra info to the item
        /// </summary>
        /// <param name="info">The info to apply</param>
        internal virtual void PrepareItem(TreeListBoxInfo info)
        {
            //set the current info for this object
            currentInfo = info;
            //set the level
            level = info.Level;
            //check if the item was expanded. 
            //you need to reset this propety since the Virtualization can delete the instance of this item and after recreate it
            if (info.IsExpanded)
                IsExpanded = true;

            isInitializing = false;
        }

        #region Expand Collapse

        //expands or collapses the item
        private void ExpandCollapseItem()
        {
            //set the expanded property
            currentInfo.IsExpanded = IsExpanded;

            if (IsExpanded)
                GenerateAllListItems();
            else
                DropAllListItems();
        }

        //generates all the child list items
        private void GenerateAllListItems()
        {
            IEnumerable list = GetChildList();

            if (list != null)
            {
                //generate a child for each item in the source
                foreach (object item in list)
                    GenerateSingleItem(item);
            }
        }

        /// <summary>
        /// drops all the child list items
        /// </summary>
        internal void DropAllListItems()
        {
            if (currentInfo.ChildItems != null)
            {
                for (int i = 0; i < currentInfo.ChildItems.Count; i++)
                {
                    if (DeleteSingleItem(currentInfo.ChildItems[i]))
                        i--;//set -- since this method deletes from the dataItems collection
                }

                //unregister to events
                UnRegisterCollectionNotification();

                lastChildItemCreatedIndex = 1;//reset the counter
            }
        }

        //get the list of children
        private IEnumerable GetChildList()
        {
            IEnumerable otherList = null;

            if (!String.IsNullOrEmpty(treeListBox.ChildItemSourcePath))
            {
                //get the property info for the child collection
                if (childCollectionProperty == null)
                {
                    childCollectionProperty =
                        ReflectionHelper.GetPropertyForObject(currentInfo.DataItem, treeListBox.ChildItemSourcePath);
                    if (childCollectionProperty == null)
                        throw new InvalidOperationException("The ChildItemSourcePath specified is invalid");
                    if (!ReflectionHelper.InterfacePresentInType(childCollectionProperty.PropertyType, typeof(IEnumerable)))
                        throw new InvalidOperationException("The ChildItemSourcePath must be of type IEnumerable");
                }

                //first check if the value is null without doing any casting
                object value = childCollectionProperty.GetValue(currentInfo.DataItem, null);
                if (value == null)
                    return null;

                otherList = value as IEnumerable;

                //check if the child collection supports INotifyCollectionChange and if it does listen to events
                childrenNotificationList = otherList as INotifyCollectionChanged;
                if (childrenNotificationList != null)
                {
                    if (notifyCollectionChangedEventHandler == null)
                        notifyCollectionChangedEventHandler = NotificationEnabledCollectionCollectionChanged;

                    childrenNotificationList.CollectionChanged += notifyCollectionChangedEventHandler;
                }

            }
            return otherList;
        }

        //event handler for databinding notification fo children
        private void NotificationEnabledCollectionCollectionChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            //if the expanded is false. Unregister to events and exit 
            //this can happen if the Drop all does not unregister to notification events
            if (currentInfo.IsExpanded == false)
            {
                UnRegisterCollectionNotification();
                return;
            }

            switch (e.Action)
            {
                //new item(s) where added
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        int counter = 0;
                        foreach (object item in e.NewItems)
                        {
                            int originalDestinationIndex =
                                Math.Min(e.NewStartingIndex + counter, currentInfo.ChildItems.Count);

                            //get the index for the main list (the flattened list)
                            int newDestinationIndex = 1;
                            //walk through all ancestor items and get te child count of the items
                            for (int i = originalDestinationIndex - 1; i >= 0; i--)
                                //get the child count of the item + 1 which is the item itself
                                newDestinationIndex += (currentInfo.ChildItems[i].ChildrenCount + 1);

                            originalDestinationIndex =
                                newDestinationIndex + // the new destination
                                treeListBox.CompositeChildCollection.IndexOf(currentInfo);// the index of the current item

                            //create the new list item
                            TreeListBoxInfo newInfo = new TreeListBoxInfo(level + 1, item);

                            currentInfo.ChildItems.Insert(
                                Math.Min(e.NewStartingIndex + counter, currentInfo.ChildItems.Count),
                                newInfo);

                            treeListBox.CompositeChildCollection.Insert(originalDestinationIndex, newInfo);

                            counter++;
                        }
                    }
                    break;

                //item was removed
                case NotifyCollectionChangedAction.Remove:
                    if (currentInfo.ChildItems.Count > e.OldStartingIndex)
                        DeleteSingleItem(currentInfo.ChildItems[e.OldStartingIndex]);
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    DropAllListItems();
                    GenerateAllListItems();
                    break;
            }
        }

        //unregisters to collection notifications
        private void UnRegisterCollectionNotification()
        {
            if (notifyCollectionChangedEventHandler != null && childrenNotificationList != null)
                childrenNotificationList.CollectionChanged -= notifyCollectionChangedEventHandler;
        }

        //generate a single TreeListBoxItem
        private void GenerateSingleItem(object item)
        {
            //get the current index. TODO done once
            int currentIndex = treeListBox.CompositeChildCollection.IndexOf(currentInfo);

            if (currentIndex != -1)
            {
                //create a child list item
                TreeListBoxInfo newObject = new TreeListBoxInfo(level + 1, item);
                currentInfo.ChildItems.Add(newObject);
                treeListBox.CompositeChildCollection.Insert(
                    currentIndex + lastChildItemCreatedIndex, newObject);
                lastChildItemCreatedIndex++;
            }
        }

        //delets a single item from the list
        private bool DeleteSingleItem(TreeListBoxInfo info)
        {
            int index = treeListBox.CompositeChildCollection.IndexOf(info);
            if (index != -1)
            {
                //remove all children
                for (int i = 0; i < info.ChildrenCount; i++)
                {
                    treeListBox.CompositeChildCollection[index].IsExpanded = false;
                    treeListBox.CompositeChildCollection.RemoveAt(index);
                }
                treeListBox.CompositeChildCollection[index].IsExpanded = false;
                treeListBox.CompositeChildCollection.RemoveAt(index);//remove itself

                currentInfo.ChildItems.Remove(info);
                lastChildItemCreatedIndex--;
                return true;
            }
            return false;
        }
        #endregion

        //the indentation size
        private const double IndentSize = 19.0;

        /// <summary>
        /// Apply the control template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ToggleButton expander = GetTemplateChild("PART_Header") as ToggleButton;
            if (expander != null)
            {
                //bind the IsExpanded property
                Binding expandedBinding = new Binding("IsExpanded");
                expandedBinding.Source = this;
                expandedBinding.Mode = BindingMode.TwoWay;
                expander.SetBinding(ToggleButton.IsCheckedProperty, expandedBinding);

                //create the indentation for the item
                expander.Margin = new Thickness(level * IndentSize, 0, 0, 0);
            }
        }

        #region Unit Tests
        /// <summary>
        /// Esposes the PrepareItem
        /// </summary>
        /// <param name="info"></param>
        [System.Diagnostics.Conditional("UNIT_TESTS")]
        public void ExposePrepareItem(TreeListBoxInfo info)
        {
            PrepareItem(info);
        }
        #endregion
    }
}
