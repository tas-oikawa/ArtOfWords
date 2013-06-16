using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
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

namespace TagsGrooveControls.View
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class TagsGrooveTreeView : UserControl
    {
        public TagsGrooveTreeView()
        {
            InitializeComponent();
        }

        private void _addChildButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (sender as FrameworkElement).DataContext as TagTreeViewItemModel;

            GetModel().AddChild(model);
        }

        private void _deleteChildButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (sender as FrameworkElement).DataContext as TagTreeViewItemModel;

            if (MessageBox.Show(model.Name + "を削除してもいいですか？", "確認", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            GetModel().Remove(model);
        }

        private TagsGrooveTreeViewModel GetModel()
        {
            return this.DataContext as TagsGrooveTreeViewModel;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var model = (sender as FrameworkElement).DataContext as TagTreeViewItemModel;

            if (model.IsSelected)
            {
                model.IsNameMode = true;

                _lastMouseDown = e.GetPosition(TagTreeView);
            }
        }

        #region Drag And Drop
    
        Point _lastMouseDown;
        TagModel draggedItem, _target;

        private void treeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(TagTreeView);
            }
        }

        private bool IsMouseMovedSignificantly(Point position)
        {
            if ((Math.Abs(position.X - _lastMouseDown.X) <= 10.0) &&
                (Math.Abs(position.Y - _lastMouseDown.Y) <= 10.0))
            {
                return false;
            }

            return true;
        }

        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    return;
                }


                Point currentPosition = e.GetPosition(TagTreeView);
                
                if (_lastMouseDown.X == 0.0 && _lastMouseDown.Y == 0.0)
                {
                    _lastMouseDown.X = currentPosition.X;
                    _lastMouseDown.Y = currentPosition.Y;

                    return;
                }

                if (!IsMouseMovedSignificantly(currentPosition))
                {
                    return;
                }
                _lastMouseDown = currentPosition;

                draggedItem =  GetModel().GetSelectingTag();

                if (draggedItem == null)
                {
                    return;
                }

                (draggedItem as TagTreeViewItemModel).IsNameMode = false;
                

                DragDropEffects finalDropEffect = DragDrop.DoDragDrop(TagTreeView, TagTreeView.SelectedValue,
                    DragDropEffects.Move);
                //Checking target is not null and item is dragging(moving)
                if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                {
                    // A Move drop was accepted
                    if (CheckDropTarget(draggedItem, _target))
                    {
                        GetModel().ChangeParent(_target, draggedItem);
                        _target = null;
                        draggedItem = null;
                    }       

                }
            }
            catch (Exception)
            {
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Point currentPosition = e.GetPosition(TagTreeView);

                if (IsMouseMovedSignificantly(currentPosition))
                {
                    // Verify that this is a valid drop and then store the drop target
                    TagModel item = GetNearestContainer(e.OriginalSource as UIElement);
                    if (CheckDropTarget(draggedItem, item))
                    {
                        e.Effects = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }

        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                var TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (TargetItem != null && draggedItem != null )
                {
                    _target = TargetItem as TagModel;
                    e.Effects = DragDropEffects.Move;

                }
            }
            catch (Exception)
            {
            }



        }
        private bool CheckDropTarget(TagModel sourceItem, TagModel targetItem)
        {
            if (sourceItem.Id == targetItem.Id)
            {
                return false;
            }

            if(sourceItem.HasInDescendent(targetItem))
            {
                return false;
            }

            return true;

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
        private TagModel GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container.DataContext as TagModel;
        }

        #endregion

    }
}
