using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TagsGrooveControls.View;

namespace TagsGrooveControls.Model
{
    public class TagsGrooveTreeViewModel : INotifyPropertyChanged
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

        public void Remove(Tag deleteTarget)
        {
            _manager.Remove(deleteTarget);

            OnPropertyChanged("Tags");
            OnTagRemoved(deleteTarget);
        }

        public Tag GetSelectingTag()
        {
            var selected = _view.TagTreeView.SelectedItem;

            if (selected == null)
            {
                return null;
            }

            return selected as Tag;
        }

        public void ChangeParent(Tag target, Tag draggedItem)
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

        public delegate void RemoveTagEventHandler(object sender ,Tag deleteTag);
        public event RemoveTagEventHandler TagRemoved;
        public void OnTagRemoved(Tag tag)
        {
            if (TagRemoved != null)
            {
                TagRemoved(this, tag);
            }
        }
    }
}
