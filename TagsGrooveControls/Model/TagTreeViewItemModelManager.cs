using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class TagTreeViewItemModelManager : TagManager
    {
        private List<Tag> _removedTagList;

        public TagTreeViewItemModelManager()
        {
            _removedTagList = new List<Tag>();
        }

        protected override void Initialize()
        {
        }


        protected override int GetNewId()
        {
            int tempMaxId = base.GetNewId();

            if (_removedTagList.Count() > 0)
            {
                return Math.Max(tempMaxId, _removedTagList.Max((e) => e.Id)) + 1;
            }

            return tempMaxId;
        }

        public override Tag GenerateNewTag()
        {
            return new TagTreeViewItemModel(new Tag(GetNewId())) { Name = "名前の無いタグ" };
        }

        public override void Remove(Tag tag, RemoveType type)
        {
            base.Remove(tag, type);
            _removedTagList.Add(tag);
        }
    }
}
