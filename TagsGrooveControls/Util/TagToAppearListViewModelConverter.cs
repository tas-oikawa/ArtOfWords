using CommonControls.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TagsGrooveControls.Util
{
    public static class TagToAppearListViewModelConverter
    {
        public static ObservableCollection<AppearListViewItemModel> ToAppearListViewItemModel(IEnumerable<int> tagIds, TagManager manager)
        {
            var tagItemModels = new ObservableCollection<AppearListViewItemModel>();

            foreach (var tagId in tagIds)
            {
                var tag = manager.TagDictionary[tagId];
                tagItemModels.Add(new AppearListViewItemModel(
                    tag.Name,
                    true,
                    "",
                    "",
                    tag) { BackgroundBrush = Brushes.LemonChiffon });
            }

            return tagItemModels;
        }
    }
}
