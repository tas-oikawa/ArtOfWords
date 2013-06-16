using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public static class TagManagerReverseConverter
    {
        public static void Reflect(TagManager dest, TagTreeViewItemModelManager source)
        {
            foreach (var deletedTag in source.DeletedTags)
            {
                dest.Remove(deletedTag.Id);
            }

            dest.ReconnectAllToBaseTag();

            foreach (var tag in source.AddedTags)
            {
                var addNewTag = new TagModel(tag.Id) { Name = tag.Name };
                dest.Add(addNewTag);
                dest.ConnectTags(dest.GetBaseTag(), addNewTag);
            }

            foreach (var tag in dest.TagDictionary.Values)
            {
                if (tag.IsBase())
                {
                    continue;
                }

                var sourceTag = source.TagDictionary[tag.Id];

                var parent = dest.TagDictionary[sourceTag.Parent.Id];

                dest.DisconnectFromParent(tag);
                dest.ConnectTags(parent, tag);
                tag.Name = sourceTag.Name;
            }
        }
    }
}
