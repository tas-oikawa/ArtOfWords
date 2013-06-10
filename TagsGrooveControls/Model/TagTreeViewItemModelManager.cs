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

        public TagTreeViewItemModelManager()
        {
            _deletedTags = new List<TagTreeViewItemModel>();
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

        public override void Remove(Tag tag, RemoveType type)
        {
            base.Remove(tag as Tag, type);

            _deletedTags.Add(tag as TagTreeViewItemModel);
        }


        public override Tag GenerateNewTag()
        {
            return new TagTreeViewItemModel(GetNewId()) { Name = "名前の無いタグ" };
        }
    }
}
