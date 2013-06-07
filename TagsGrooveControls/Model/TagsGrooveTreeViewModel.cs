using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace TagsGrooveControls.Model
{
    public class TagsGrooveTreeViewModel : INotifyPropertyChanged
    {
        private TagManager _manager;
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
            _manager = TempTagManagerFactory.Generate(tagManager);
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

        public void Remove(TagTreeViewItemModel deleteTarget)
        {
            if (MessageBox.Show(deleteTarget.Name + "を削除してもいいですか？", "確認", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            _manager.Remove(deleteTarget, RemoveType.RemoveRelatedChildren);

            OnPropertyChanged("Tags");
            OnTagRemoved(deleteTarget);
        }

        public void SetNewParent(TagTreeViewItemModel parentTarget, TagTreeViewItemModel childTarget)
        {
            _manager.DisconnectFromParent(childTarget);
            _manager.ConnectTags(parentTarget, childTarget);

            parentTarget.IsExpanded = true;
        }

        public TagTreeViewItemModel GetSelectedTagFromChild(TagTreeViewItemModel childTag)
        {
            foreach (var child in childTag.Children)
            {
                var tag = child as TagTreeViewItemModel;
                if (tag.IsSelected)
                {
                    return tag;
                }

                var childSelected = GetSelectedTagFromChild(tag);
                if(childSelected != null)
                {
                    return childSelected;
                }
            }
            return null;
        }


        public TagTreeViewItemModel FindSelectedTag()
        {
            foreach (var tag in _tags)
            {
                if (tag.IsSelected)
                {
                    return tag;
                }
                TagTreeViewItemModel selectedTag = GetSelectedTagFromChild(tag);

                if (selectedTag != null)
                {
                    return selectedTag;
                }
            }
            return null;
        }

        public void InsertTagIfExistSelectedTag()
        {
            var selectedTag = FindSelectedTag();
            if (selectedTag == null)
            {
                return;
            }

            AddChild(selectedTag);
        }

        public void RemoveTagIfExistSelectedTag()
        {
            var selectedTag = FindSelectedTag();
            if (selectedTag == null)
            {
                return;
            }

            Remove(selectedTag);
        }

        public void GoNameModeIfExistSelectedTag()
        {
            var selectedTag = FindSelectedTag();
            if (selectedTag == null)
            {
                return;
            }

            GoNameMode(selectedTag);
        }

        public void GoNameMode(TagTreeViewItemModel tag)
        {
            if (tag.IsSelected)
            {
                tag.IsNameMode = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public delegate void RemoveTagEventHandler(object sender, TagTreeViewItemModel deleteTag);
        public event RemoveTagEventHandler TagRemoved;
        public void OnTagRemoved(TagTreeViewItemModel tag)
        {
            if (TagRemoved != null)
            {
                TagRemoved(this, tag);
            }
        }
    }
}
