using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Tag
{
    public static class TagsToString
    {
        public static string ToString(IEnumerable<int> tagIds, TagManager manager)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var tagId in tagIds)
            {
                var tag = manager.TagDictionary[tagId];
                builder.Append(tag.Name + ",");
            }

            return builder.ToString();
        }
    }
}
