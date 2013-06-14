using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class TagTreeViewItemModelManager : TagManager
    {
        private List<TagTreeViewItemModel> _deletedTags;

        public List<TagTreeViewItemModel> DeletedTags
        {
            get { return _deletedTags; }
            set { _deletedTags = value; }
        }

        private List<TagTreeViewItemModel> _addedTags;

        public List<TagTreeViewItemModel> AddedTags
        {
            get { return _addedTags; }
            set { _addedTags = value; }
        }

        public TagTreeViewItemModelManager()
        {
            _deletedTags = new List<TagTreeViewItemModel>();
            _addedTags = new List<TagTreeViewItemModel>();
        }

        protected override int GetNewId()
        {
            int _maxId = base.GetNewId();
            int _deletedMaxId = 0;

            if (_deletedTags.Count > 0)
            {
                _deletedMaxId = _deletedTags.Max((e) => e.Id);
            }

            return Math.Max(_maxId, _deletedMaxId) + 1;
        }

        protected override void RemoveFromDictonary(Tag tag)
        {
            base.RemoveFromDictonary(tag as Tag);

            if (_addedTags.Contains(tag))
            {
                _addedTags.Remove(tag as TagTreeViewItemModel);
            }
            else
            {
                _deletedTags.Add(tag as TagTreeViewItemModel);
            }

        }

        public override Tag GenerateNewTag()
        {
            var newTag = new TagTreeViewItemModel(GetNewId()) { Name = "名前の無いタグ" };
            _addedTags.Add(newTag);

            return newTag;
        }

        public bool IsDeleted(Tag tag)
        {
            return _deletedTags.Find((e) => e.Id == tag.Id) != null;
        }
    }
}
