using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagsGrooveControls.Model;

namespace TagsGrooveControls
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class TagsGrooveTreeView : UserControl
    {
        public readonly static RoutedCommand InputName =
            new RoutedCommand("InputName", typeof(TagsGrooveTreeView));
        public readonly static RoutedCommand InsertTag =
            new RoutedCommand("InsertTag", typeof(TagsGrooveTreeView));
        public readonly static RoutedCommand DeleteTag =
            new RoutedCommand("DeleteTag", typeof(TagsGrooveTreeView));

        Point _lastMouseDown;
        TagTreeViewItemModel draggedItem, _target;

        public TagsGrooveTreeView()
        {
            InitializeComponent();
            initCommandBindings();
        }

        private void initCommandBindings()
        {
            this.CommandBindings.Add(new CommandBinding(InputName, (s, e) => { GetModel().GoNameModeIfExistSelectedTag(); }, (s, e) => e.CanExecute = true));
            this.CommandBindings.Add(new CommandBinding(InsertTag, (s, e) => { GetModel().InsertTagIfExistSelectedTag(); }, (s, e) => e.CanExecute = true));
            this.CommandBindings.Add(new CommandBinding(DeleteTag, (s, e) => { GetModel().RemoveTagIfExistSelectedTag(); }, (s, e) => e.CanExecute = true));
        }

        public TagTreeViewItemModel GetSelectedModel()
        {
            return GetModel().FindSelectedTag();
        }

        private void _addChildButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (sender as Button).DataContext as TagTreeViewItemModel;

            GetModel().AddChild(model);
        }

        private void _deleteChildButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (sender as Button).DataContext as TagTreeViewItemModel;

            GetModel().Remove(model);
        }

        private TagsGrooveTreeViewModel GetModel()
        {
            return this.DataContext as TagsGrooveTreeViewModel;
        }

        private void TextBox_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var model = (sender as TextBlock).DataContext as TagTreeViewItemModel;
            GetModel().GoNameMode(model);
        }

        private void TextBox_IsVisibleChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            textBox.Focus();
        }

        private void ExecuteInput(object sender, ExecutedRoutedEventArgs e)
        {
            GetModel().GoNameModeIfExistSelectedTag();
        }

        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(TagTreeView);
            }
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            Point currentPosition = e.GetPosition(TagTreeView);

            if ((Math.Abs(currentPosition.X - _lastMouseDown.X) <= 10.0) &&
                (Math.Abs(currentPosition.Y - _lastMouseDown.Y) <= 10.0))
            {
                return;
            }
            StartDrag();
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Point currentPosition = e.GetPosition(TagTreeView);

                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) <= 10.0) &&
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) <= 10.0))
                {
                    return;
                }

                // Verify that this is a valid drop and then store the drop target
                TagTreeViewItemModel item = GetNearestContainer(e.OriginalSource as UIElement);
                if (IsSameItem(draggedItem, item))
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }

                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        private bool IsSameItem(TagTreeViewItemModel _sourceItem, TagTreeViewItemModel _targetItem)
        {
            return (_sourceItem.Id == _targetItem.Id);
        }

        private bool IsChildItem(TagTreeViewItemModel _sourceItem, TagTreeViewItemModel _targetItem)
        {
            return _sourceItem.IsChild(_targetItem);
        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                TagTreeViewItemModel TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedItem != null)
                {
                    _target = TargetItem;
                    e.Effects = DragDropEffects.Move;
                }
            }
            catch (Exception)
            {
            }
        }

        static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                TObject found = parent as TObject;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }

        private TagTreeViewItemModel GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }

            return container.DataContext as TagTreeViewItemModel;
        }

        private void StartDrag()
        {
            draggedItem = GetModel().FindSelectedTag();

            if (draggedItem == null)
            {
                return;
            }

            try
            {
                DragDropEffects finalDropEffect = DragDrop.DoDragDrop(TagTreeView,
                                    TagTreeView.SelectedValue,
                                    DragDropEffects.Move);

                if ((finalDropEffect != DragDropEffects.Move))
                {
                    return;
                }

                if (_target == null)
                {
                    return;
                }

                // A Move drop was accepted
                if (IsSameItem(_target, draggedItem) || IsChildItem(draggedItem, _target ))
                {
                    return;
                }

                GetModel().SetNewParent(_target, draggedItem);

                _target = null;
                draggedItem = null;
            }
            catch (Exception)
            {
            }
        }
    }
}
