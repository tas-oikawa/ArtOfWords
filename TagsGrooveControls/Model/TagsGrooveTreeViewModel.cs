using GongSolutions.Wpf.DragDrop;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using TagsGrooveControls.View;

namespace TagsGrooveControls.Model
{
    public class TagsGrooveTreeViewModel : INotifyPropertyChanged,  IDropTarget
    {
        private TagTreeViewItemModelManager _manager;

        public TagTreeViewItemModelManager Manager
        {
            get { return _manager; }
            set { _manager = value; }
        }
        private ObservableCollection<TagTreeViewItemModel> _tags;

        public ObservableCollection<TagTreeViewItemModel> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        private TagsGrooveTreeView _view;

        public TagsGrooveTreeViewModel(TagsGrooveTreeView view)
        {
            _view = view;
            _view.DataContext = this;
        }

        public void Init(TagManager tagManager)
        {
            _manager = TagManagerConverter.Convert(tagManager);
            var baseTag = _manager.GetBaseTag();

            _tags = new ObservableCollection<TagTreeViewItemModel>();

            _tags.Add(baseTag as TagTreeViewItemModel);
        }

        public void AddChild(TagTreeViewItemModel addTarget)
        {
            var newTag = _manager.GenerateNewTag() as TagTreeViewItemModel;

            _manager.ConnectTags(addTarget, newTag);
            _manager.Add(newTag);

            newTag.IsSelected = true;
            addTarget.IsSelected = false;
            addTarget.IsExpanded = true;

            OnPropertyChanged("Tags");
        }

        public void Remove(TagModel deleteTarget)
        {
            _manager.Remove(deleteTarget);

            OnPropertyChanged("Tags");
            OnTagRemoved(deleteTarget);
        }

        public TagModel GetSelectingTag()
        {
            var selected = _view.TagTreeView.SelectedItem;

            if (selected == null)
            {
                return null;
            }

            return selected as TagModel;
        }

        public void ChangeParent(TagModel target, TagModel draggedItem)
        {
            _manager.DisconnectFromParent(draggedItem);
            _manager.ConnectTags(target, draggedItem);

            var modelItem = target as TagTreeViewItemModel;
            modelItem.IsExpanded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public delegate void RemoveTagEventHandler(object sender ,TagModel deleteTag);
        public event RemoveTagEventHandler TagRemoved;
        public void OnTagRemoved(TagModel tag)
        {
            if (TagRemoved != null)
            {
                TagRemoved(this, tag);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is TagTreeViewItemModel) || !(dropInfo.TargetItem is TagTreeViewItemModel))
            {
                return;
            }

            var dragData = dropInfo.Data as TagTreeViewItemModel;
            var targetData = dropInfo.TargetItem as TagTreeViewItemModel;
            
            if (!CheckDropTarget(dragData, targetData))
            {
                return;
            }

            dropInfo.Effects = DragDropEffects.Move;
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is TagTreeViewItemModel) || !(dropInfo.TargetItem is TagTreeViewItemModel))
            {
                return;
            }

            var dragData = dropInfo.Data as TagTreeViewItemModel;
            var targetData = dropInfo.TargetItem as TagTreeViewItemModel;

            if (!CheckDropTarget(dragData, targetData))
            {
                return;
            }

            ChangeParent(targetData, dragData);
        }

        private bool CheckDropTarget(TagModel sourceItem, TagModel targetItem)
        {
            if (sourceItem.Id == targetItem.Id)
            {
                return false;
            }

            if (sourceItem.HasInDescendent(targetItem))
            {
                return false;
            }

            return true;

        }
    }
}
