using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TagsGrooveControls.View;

namespace TagsGrooveControls.Model
{
    public class TagSelectorViewModel
    {
        private TagsGrooveTreeViewModel _treeViewModel;
        private TagsGrooveTreeView _treeView;

        private ObservableCollection<Tag> _selectingTags;

        public ObservableCollection<Tag> SelectingTags
        {
            get { return _selectingTags; }
            set { _selectingTags = value; }
        }

        public TagSelectorViewModel()
        {
            _selectingTags = new ObservableCollection<Tag>();
        }

        public void SetView(TagsGrooveTreeView treeView)
        {
            _treeView = treeView;

            _treeViewModel = new TagsGrooveTreeViewModel(_treeView);

            _treeViewModel.Init(ModelsComposite.TagManager);
        }

        public void AddSelection()
        {
            var selectedTag = _treeViewModel.GetSelectingTag();

            if (selectedTag == null)
            {
                return;
            }

            if (selectedTag.IsBase())
            {
                return;
            }

            if (_selectingTags.Contains(selectedTag))
            {
                return;
            }

            SelectingTags.Add(selectedTag);
        }

        public void RemoveSelection(Tag selectingTag)
        {
            if (selectingTag == null)
            {
                return;
            }

            if (!_selectingTags.Contains(selectingTag))
            {
                return;
            }

            SelectingTags.Remove(selectingTag);
        }
    }
}
