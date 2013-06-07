using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public static class TempTagManagerFactory
    {
        public static TagTreeViewItemModel Generate(Tag tag, TagManager manager)
        {
            TagTreeViewItemModel model = new TagTreeViewItemModel(tag);

            manager.Add(model);
            foreach (var child in tag.Children)
            {
                var childModel = Generate(child, manager);

                manager.ConnectTags(model, childModel);
            }

            return model;
        }

        public static TagTreeViewItemModelManager Generate(TagManager source)
        {
            var tagManager = new TagTreeViewItemModelManager();

            Generate(source.GetBaseTag(), tagManager);

            return tagManager;
        }
    }
}
