using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagsGrooveControls.Util;

namespace TagsGrooveControls.Model
{
    public static class TagManagerConverter
    {
        public static void ExpandChild(TagTreeViewItemModel parentTag, TagModel baseTag, ref TagTreeViewItemModelManager manager)
        {
            foreach (var child in baseTag.Children)
            {
                var childTag = ConvertTagToTagTreeViewItemModel.ConvertTag(child);

                manager.Add(childTag);
                manager.ConnectTags(parentTag, childTag);

                ExpandChild(childTag, child, ref manager);
            }
        }

        public static TagTreeViewItemModelManager Convert(TagManager source)
        {
            TagTreeViewItemModelManager result = new TagTreeViewItemModelManager();

            var baseTag = source.GetBaseTag();

            var addBaseTag = ConvertTagToTagTreeViewItemModel.ConvertBaseTag(baseTag);

            result.Add(addBaseTag);
            ExpandChild(addBaseTag, baseTag, ref result);

            return result;
        }
    }
}
