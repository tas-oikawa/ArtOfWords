using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Markup;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// Container in which it's children can be dragged around
    /// </summary>
    [ContentProperty("Children")]
    public class DockableContainer : Control, IAddChild
    {
        #region Properties

        /// <summary>
        /// Gets or sets the children of the collection
        /// </summary>
        public ObservableCollection<UIElement> Children
        {
            get { return (ObservableCollection<UIElement>)GetValue(ChildrenProperty); }
            private set { SetValue(ChildrenProperty, value); }
        }

        /// <summary>
        /// Gets or sets the children of the collection
        /// </summary>
        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.Register("Children", typeof(ObservableCollection<UIElement>), typeof(DockableContainer), new UIPropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Side panel width
        /// </summary>
        public double SidePanelWidth
        {
            get { return (double)GetValue(SidePanelWidthProperty); }
            set { SetValue(SidePanelWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Side panel width
        /// </summary>
        public static readonly DependencyProperty SidePanelWidthProperty =
            DependencyProperty.Register("SidePanelWidth", typeof(double), typeof(DockableContainer), new UIPropertyMetadata(0.0));



        /// <summary>
        /// Gets or sets the Side panel height
        /// </summary>
        public double SidePanelHeight
        {
            get { return (double)GetValue(SidePanelHeightProperty); }
            set { SetValue(SidePanelHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Side panel height
        /// </summary>
        public static readonly DependencyProperty SidePanelHeightProperty =
            DependencyProperty.Register("SidePanelHeight", typeof(double), typeof(DockableContainer), new UIPropertyMetadata(0.0));


        #region Docking Properties

        private static bool GetIsDocked(DependencyObject obj)
        {
            if (obj.GetValue(IsDockedProperty) == null)
                return false;
            return (bool)obj.GetValue(IsDockedProperty);
        }

        private static void SetIsDocked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDockedProperty, value);
        }

        private static readonly DependencyProperty IsDockedProperty =
            DependencyProperty.RegisterAttached("IsDocked", typeof(bool), typeof(DockableContainer), new UIPropertyMetadata(true));


        /// <summary>
        /// Attached property that can be used by the children of this control to specify where they want to e docked
        /// </summary>
        public static Nullable<Dock> GetDock(DependencyObject obj)
        {
            return (Nullable<Dock>)obj.GetValue(DockProperty);
        }

        /// <summary>
        /// Attached property that can be used by the children of this control to specify where they want to e docked
        /// </summary>
        public static void SetDock(DependencyObject obj, Nullable<Dock> value)
        {
            obj.SetValue(DockProperty, value);
        }

        /// <summary>
        /// Attached property that can be used by the children of this control to specify where they want to e docked
        /// </summary>
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.RegisterAttached("Dock", typeof(Nullable<Dock>), typeof(DockableContainer));

        #endregion


        /// <summary>
        /// Gets the element to be dragged from the child element that sets this property
        /// </summary>
        /// <param name="obj">The element that stores this property</param>
        /// <returns>Returns the element to be dragged</returns>
        public static FrameworkElement GetElementForDragging(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(ElementForDraggingProperty);
        }

        /// <summary>
        /// Sets the element to be dragged
        /// </summary>
        /// <param name="obj">The element to store the property</param>
        /// <param name="value">The element to be dragged</param>
        public static void SetElementForDragging(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(ElementForDraggingProperty, value);
        }

        /// <summary>
        /// Gets or sets the element to drag when undocked
        /// </summary>
        public static readonly DependencyProperty ElementForDraggingProperty =
            DependencyProperty.RegisterAttached("ElementForDragging", typeof(FrameworkElement), typeof(DockableContainer), new UIPropertyMetadata(null));

        #endregion

        #region DataMembers
        DockPanel top, bottom, left, right;
        Panel body;
        Canvas draggingSurface;

        FrameworkElement elementBeingDragged;
        bool itemsAddedBeforeIntialize = false;
        Point originalPointBeingDragged;
        #endregion

        #region Commands
        /// <summary>
        /// Command to Dock the control or un dock it
        /// </summary>
        public static RoutedUICommand ToggleDockChild;
        #endregion

        /// <summary>
        /// Static constructor
        /// </summary>
        static DockableContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DockableContainer), new FrameworkPropertyMetadata(typeof(DockableContainer)
                    ));

            ToggleDockChild = new RoutedUICommand("ToggleDockChild", "ToggleDockChild", typeof(DockableContainer));

            CommandManager.RegisterClassCommandBinding(typeof(DockableContainer),
                new CommandBinding(ToggleDockChild, DockUnDockElement));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DockableContainer()
        {
            InializeProperties();

            EventHandler handler = delegate
            {
                SidePanelHeight = ActualHeight - (top.ActualHeight + bottom.ActualHeight);
                SidePanelWidth = ActualWidth - (left.ActualWidth + right.ActualWidth);
            };

            //make sure to  update the height for the side panels
            DependencyPropertyDescriptor.FromProperty(ActualHeightProperty, typeof(DockableContainer)).AddValueChanged(
                this, handler);

            DependencyPropertyDescriptor.FromProperty(ActualWidthProperty, typeof(DockableContainer)).AddValueChanged(
                this, handler);


            //add all items that have been added before being loaded
            Loaded += delegate
            {
                //Add all items that were added before initialization
                if (itemsAddedBeforeIntialize)
                    foreach (UIElement element in Children)
                        AddChild(element);
            };
        }

        #region Dock and UnDock
        private static void DockUnDockElement(object sender, ExecutedRoutedEventArgs e)
        {
            DockableContainer container = (DockableContainer)sender;
            UIElement control = (UIElement)e.Parameter;
            bool isControlDocked = GetIsDocked(control);

            if (isControlDocked)
            {
                container.RemoveChild(control);
                container.draggingSurface.Children.Add(control);
            }
            else
            {
                container.draggingSurface.Children.Remove(control);
                container.AddChild(control);
            }

            //change the state in the control
            SetIsDocked(control, !isControlDocked);
        }
        #endregion

        //initialize the children collection
        private void InializeProperties()
        {
            Children = new ObservableCollection<UIElement>();
            Children.CollectionChanged += ChildrenCollectionChanged;
        }

        //Event handler for the children collection changed. Here we put the new child in the correct place
        private void ChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (UIElement item in e.NewItems)
                        AddChild(item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (UIElement element in e.OldItems)
                        RemoveChild(element);
                    break;
                default:
                    throw new NotSupportedException("This action is not supported");
            }
        }

        /// <summary>
        /// Get all the elements for the Control Template
        /// </summary>
        public override void OnApplyTemplate()
        {
            top = (DockPanel)GetTemplateChild("PART_Top");
            top.LastChildFill = true;
            bottom = (DockPanel)GetTemplateChild("PART_Bottom");
            bottom.LastChildFill = true;
            right = (DockPanel)GetTemplateChild("PART_Right");
            right.LastChildFill = true;
            left = (DockPanel)GetTemplateChild("PART_Left");
            left.LastChildFill = true;
            body = (Panel)GetTemplateChild("PART_Body");
            draggingSurface = (Canvas)GetTemplateChild("PART_DraggingSurface");

            EventHandler updateSizes = delegate
            {
                SidePanelHeight = ActualHeight - (top.ActualHeight + bottom.ActualHeight);
                SidePanelWidth = ActualWidth - (left.ActualWidth + right.ActualWidth);
            };

            //make sure to  update the height for the side panels
            DependencyPropertyDescriptor.FromProperty(ActualHeightProperty,
                typeof(DockPanel)).AddValueChanged(top, updateSizes);
            DependencyPropertyDescriptor.FromProperty(ActualHeightProperty,
                typeof(DockPanel)).AddValueChanged(bottom, updateSizes);
            DependencyPropertyDescriptor.FromProperty(ActualWidthProperty,
                typeof(DockPanel)).AddValueChanged(left, updateSizes);
            DependencyPropertyDescriptor.FromProperty(ActualWidthProperty,
                typeof(DockPanel)).AddValueChanged(right, updateSizes);

            base.OnApplyTemplate();
        }

        #region IAddChild Members

        /// <summary>
        /// Adds a child to the children collection
        /// </summary>
        /// <param name="value">The value to add</param>
        public void AddChild(object value)
        {
            UIElement element = value as UIElement;
            if (element == null)
                throw new NotSupportedException("Only UIElement can be added to the collection");

            if (IsInitialized)
            {
                RegisterDraggingOperation(element);

                Nullable<Dock> controlDock = GetDock(element);
                if (!controlDock.HasValue)
                {
                    body.Children.Add(element);
                }
                else
                {
                    switch (controlDock)
                    {
                        case Dock.Bottom:
                            element.SetValue(DockPanel.DockProperty, Dock.Left);
                            bottom.Children.Add(element);
                            break;
                        case Dock.Left:
                            element.SetValue(DockPanel.DockProperty, Dock.Top);
                            left.Children.Add(element);
                            break;
                        case Dock.Right:
                            element.SetValue(DockPanel.DockProperty, Dock.Top);
                            right.Children.Add(element);
                            break;
                        case Dock.Top:
                            element.SetValue(DockPanel.DockProperty, Dock.Left);
                            top.Children.Add(element);
                            break;
                        default:
                            body.Children.Add(element);
                            break;
                    }
                }
            }
            else
            {
                //flag this so that we add the items later
                itemsAddedBeforeIntialize = true;
            }
        }

        //remove an item from the list
        private void RemoveChild(UIElement element)
        {
            Dock controlDock = (Dock)element.GetValue(DockProperty);
            switch (controlDock)
            {
                case Dock.Bottom:
                    bottom.Children.Remove(element);
                    break;
                case Dock.Left:
                    left.Children.Remove(element);
                    break;
                case Dock.Right:
                    right.Children.Remove(element);
                    break;
                case Dock.Top:
                    top.Children.Remove(element);
                    break;
                default:
                    body.Children.Remove(element);
                    break;
            }
        }

        /// <summary>
        /// This is not supported. Only UI Element can be added
        /// </summary>
        /// <param name="text"></param>
        public void AddText(string text)
        {
            throw new NotSupportedException("Only UIElement can be added to the collection");
        }

        #endregion

        #region Dragging Operations

        //get the element to drag
        private Visual GetChildForDragging(Visual element)
        {
            Visual elementToDrag = GetElementForDragging(element);
            if (elementToDrag != null)
                return element;

            int children = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < children; i++)
            {
                Visual child = (Visual)VisualTreeHelper.GetChild(element, i);

                Visual childElementToDrag = GetElementForDragging(child);
                if (childElementToDrag != null)
                    return child;
                else
                {
                    Visual childOfChild = GetChildForDragging(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void RegisterDraggingOperation(UIElement element)
        {
            FrameworkElement elementState = (FrameworkElement)element;

            //if the element is not fully loaded we will not find it in the visual tree so better cater for it
            if (!elementState.IsLoaded)
            {
                elementState.Loaded += delegate
                {
                    RegisterMouseEvents(elementState);
                };
            }
            else
                RegisterMouseEvents(elementState);
        }

        private void RegisterMouseEvents(UIElement element)
        {
            UIElement elementToRegister = (UIElement)GetChildForDragging(element);

            //regsiter for dragging
            if (elementToRegister != null)
            {
                elementToRegister.PreviewMouseLeftButtonDown += DraggingElementMouseDown;
                elementToRegister.PreviewMouseUp += delegate { elementBeingDragged = null; };
            }
        }

        //register the element to drag
        void DraggingElementMouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement elementToDrag = GetElementForDragging((DependencyObject)sender);
            //only register if the control is not docked
            if (!GetIsDocked(elementToDrag))
            {
                //Mouse.Capture(elementToDrag);
                elementBeingDragged = elementToDrag;
                originalPointBeingDragged = e.GetPosition(elementBeingDragged);
            }
        }

        /// <summary>
        /// move any element that is being dragged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (elementBeingDragged != null)
            {
                Point currentPoint = e.GetPosition(this);
                Point elementPoint = e.GetPosition(elementBeingDragged);
                double x = currentPoint.X - originalPointBeingDragged.X;
                double y = currentPoint.Y - originalPointBeingDragged.Y;

                //copy a refernce to the element being dragged since we can change it if it is out of range
                FrameworkElement elementBeingDraggedLocal = elementBeingDragged;

                //validate that this is not out of screen
                if (x + elementBeingDraggedLocal.ActualWidth > ActualWidth)
                {
                    x = ActualWidth - elementBeingDraggedLocal.ActualWidth;
                    elementBeingDragged = null; //set to null nice we went of screen
                }

                //validate that this is not out of screen
                if (y + elementBeingDraggedLocal.ActualHeight > ActualHeight)
                {
                    y = ActualHeight - elementBeingDraggedLocal.ActualHeight;
                    elementBeingDragged = null; //set to null nice we went of screen
                }

                //check if x is less than 0 and make it 0
                if (x < 0)
                {
                    x = 0;
                    elementBeingDragged = null;
                }

                //check if y is less than 0 and make it 0
                if (y < 0)
                {
                    y = 0;
                    elementBeingDragged = null;
                }

                Canvas.SetLeft(elementBeingDraggedLocal, x);
                Canvas.SetTop(elementBeingDraggedLocal, y);
            }

            base.OnMouseMove(e);
        }
        /// <summary>
        /// unregister the element to drag
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            //if (elementBeingDragged != null)
            //    elementBeingDragged.ReleaseMouseCapture();
            elementBeingDragged = null;
            base.OnMouseUp(e);
        }

        #endregion
    }
}
