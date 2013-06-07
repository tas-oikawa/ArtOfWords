using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class TagSelectorViewModel : INotifyPropertyChanged
    {
        private TagSelector _view;
        private TagsGrooveTreeViewModel _treeViewModel;
        private TagsGrooveTreeView _treeView;

        private ObservableCollection<TagListItemModel> _selectedTagList;

        public ObservableCollection<TagListItemModel> SelectedTagList
        {
            get { return _selectedTagList; }
            set { _selectedTagList = value; }
        }

        public void SetView(TagSelector view)
        {
            _selectedTagList = new ObservableCollection<TagListItemModel>();

            _view = view;

            _treeView = _view._treeView;
            _treeViewModel = new TagsGrooveTreeViewModel(_treeView);

            _treeViewModel.Init(ModelsComposite.TagManager);
        }

        public void AddSelectedTag()
        {
            var tag = _treeViewModel.FindSelectedTag();

            if (tag == null || tag.IsBase())
            {
                return;
            }

            foreach (var item in _selectedTagList)
            {
                if (item.Id == tag.Id)
                {
                    return;
                }
            }

            SelectedTagList.Add(new TagListItemModel(tag));
        }

        public void RemoveTag(TagListItemModel tag)
        {
            SelectedTagList.Remove(tag);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

    }
}
