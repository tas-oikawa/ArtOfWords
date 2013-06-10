using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public static class TagManagerConverter
    {
        private static TagTreeViewItemModel ConvertTag(Tag tag)
        {
            if(tag.IsBase())
            {
                return ConvertBaseTag(tag);
            }

            var model = new TagTreeViewItemModel(tag.Id);
            model.Name = tag.Name;

            return model;
        }

        private static BaseTagTreeViewItemModel ConvertBaseTag(Tag tag)
        {
            var model = new BaseTagTreeViewItemModel(tag.Id);
            model.Name = tag.Name;

            return model;
        }

        public static void ExpandChild(TagTreeViewItemModel parentTag, Tag baseTag, ref TagTreeViewItemModelManager manager)
        {
            foreach (var child in baseTag.Children)
            {
                var childTag = ConvertTag(child);

                manager.Add(childTag);
                manager.ConnectTags(parentTag, childTag);

                ExpandChild(childTag, child, ref manager);
            }
        }

        public static TagTreeViewItemModelManager Convert(TagManager source)
        {
            TagTreeViewItemModelManager result = new TagTreeViewItemModelManager();

            var baseTag = source.GetBaseTag();

            var addBaseTag = ConvertBaseTag(baseTag);

            result.Add(ConvertBaseTag(baseTag));
            ExpandChild(addBaseTag, baseTag, ref result);

            return result;
        }
    }
}
