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


        protected override void DoPostAdd(TagModel tag)
        {
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

        protected override void RemoveFromDictonary(TagModel tag)
        {
            base.RemoveFromDictonary(tag as TagModel);

            if (_addedTags.Contains(tag))
            {
                _addedTags.Remove(tag as TagTreeViewItemModel);
            }
            else
            {
                _deletedTags.Add(tag as TagTreeViewItemModel);
            }

        }

        protected override void DoPostRemove(TagModel tag)
        {
        }

        public override TagModel GenerateNewTag()
        {
            var newTag = new TagTreeViewItemModel(GetNewId()) { Name = "名前の無いタグ" };
            _addedTags.Add(newTag);

            return newTag;
        }

        public bool IsDeleted(TagModel tag)
        {
            return _deletedTags.Find((e) => e.Id == tag.Id) != null;
        }
    }
}
