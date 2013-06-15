using CommonControls;
using CommonControls.Model;
using CommonControls.Strategy;
using CommonControls.Util;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using TagsGrooveControls.Model;
using TagsGrooveControls.Util;
using TagsGrooveControls.View;

namespace TagsGrooveControls.Strategy
{
    public class TagStickerAddButtonStrategy : IDeletableLabelAddButtonStrategy
    {
        private ITagStickable _stickable;
        private TagManager _tagManager;


        public TagStickerAddButtonStrategy(ITagStickable stickable, TagManager tagManager)
        {
            _stickable = stickable;
            _tagManager = tagManager;
        }

        public void ExecuteOnAdd(ObservableCollection<AppearListViewItemModel> dataList)
        {
            CommonLightBox dialog = new CommonLightBox();

            TagsSelectorView selectorView = new TagsSelectorView();
            var tagSelectorViewModel = new TagSelectorViewModel();
            SetSelectingTags(tagSelectorViewModel.SelectingTags);
            selectorView.SetModel(tagSelectorViewModel);

            dialog.Owner = Application.Current.MainWindow;
            dialog.BindUIElement(selectorView);
            dialog.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveCancel;

            selectorView.DataContext = tagSelectorViewModel;

            
            if (ShowDialogManager.ShowDialog(dialog) == true)
            {
                tagSelectorViewModel.UpdateModelsComposite();
                _stickable.SetTagIds(GetSelectingTagIds(tagSelectorViewModel.SelectingTags));

                dataList.Clear();
                var list = TagToAppearListViewModelConverter.ToAppearListViewItemModel(_stickable.GetTagIds(), _tagManager);
                foreach (var newItemModel in list)
                {
                    dataList.Add(newItemModel);
                }
            }

        }

        private void SetSelectingTags(ObservableCollection<Tag> selectingTag)
        {
            var ids = _stickable.GetTagIds();

            foreach (var id in ids)
            {
                var tag = _tagManager.TagDictionary[id];

                selectingTag.Add(ConvertTagToTagTreeViewItemModel.ConvertTag(tag));
            }
        }

        private List<int> GetSelectingTagIds(IEnumerable<Tag> selectingTags)
        {
            var resultIds = new List<int>();

            foreach (var tag in selectingTags)
            {
                resultIds.Add(tag.Id);
            }

            return resultIds;
        }
    }
}
