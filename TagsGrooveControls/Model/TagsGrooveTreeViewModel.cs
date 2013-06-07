using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class TagsGrooveTreeViewModel : INotifyPropertyChanged
    {
        private TagManager _manager;
        private ObservableCollection<Tag> _tags;

        public ObservableCollection<Tag> Tags
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
            _manager = tagManager;
            var baseTag = _manager.GetBaseTag();

            _tags = new ObservableCollection<Tag>();

            _tags.Add(baseTag);
        }

        public void AddChild(Tag addTarget)
        {
            var newTag = _manager.GenerateNewTag();

            _manager.ConnectTags(addTarget, newTag);
            _manager.Add(newTag);

            newTag.IsSelected = true;
            addTarget.IsSelected = false;
            addTarget.IsExpanded = true;

            OnPropertyChanged("Tags");
        }

        public void Remove(Tag deleteTarget)
        {
            _manager.Remove(deleteTarget, RemoveType.RemoveRelatedChildren);

            OnPropertyChanged("Tags");
            OnTagRemoved(deleteTarget);
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
