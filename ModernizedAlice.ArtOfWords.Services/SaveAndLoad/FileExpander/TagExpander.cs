using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad
{
    public class TagExpander
    {
        private static TagModel GetTag(SaveTagModel tagModel)
        {
            return new TagModel(tagModel.Id)
            {
                Name = tagModel.Name
            };
        }

        private static SaveTagModel GetSaveTag(TagModel tagModel)
        {
            var saveTagModel = new SaveTagModel();

            saveTagModel.Id = tagModel.Id;
            saveTagModel.Name = tagModel.Name;
            saveTagModel.ParentId = tagModel.Parent.Id;

            foreach (var child in tagModel.Children)
            {
                saveTagModel.Children.Add(child.Id);
            }

            return saveTagModel;
        }

        public static void Expand(IEnumerable<SaveTagModel> tagModels, TagManager manager)
        {
            foreach (var saveTagModel in tagModels)
            {
                manager.Add(GetTag(saveTagModel));
            }

            foreach (var saveTagModel in tagModels)
            {
                var parent = manager.TagDictionary[saveTagModel.ParentId];
                var self = manager.TagDictionary[saveTagModel.Id];

                manager.ConnectTags(parent, self);
            }
        }

        public static Collection<SaveTagModel> Expand(TagManager manager)
        {
            Collection<SaveTagModel> saveTagModelList = new Collection<SaveTagModel>();

            foreach (var tag in manager.TagDictionary.Values)
            {
                if (tag.IsBase())
                {
                    continue;
                }

                saveTagModelList.Add(GetSaveTag(tag));
            }

            return saveTagModelList;
        }
    }
}
