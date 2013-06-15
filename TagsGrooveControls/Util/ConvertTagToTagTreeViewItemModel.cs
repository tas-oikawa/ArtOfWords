using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagsGrooveControls.Model;

namespace TagsGrooveControls.Util
{
    public static class ConvertTagToTagTreeViewItemModel
    {
        public static TagTreeViewItemModel ConvertTag(Tag tag)
        {
            if (tag.IsBase())
            {
                return ConvertBaseTag(tag);
            }

            var model = new TagTreeViewItemModel(tag.Id);
            model.Name = tag.Name;

            return model;
        }

        public static BaseTagTreeViewItemModel ConvertBaseTag(Tag tag)
        {
            var model = new BaseTagTreeViewItemModel(tag.Id);
            model.Name = tag.Name;

            return model;
        }

    }
}
