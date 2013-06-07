using CommonControls.Model;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryFrameBuildControl.Model
{
    public class CharaStoryAppearListViewModelFactory
    {
        public static AppearListViewModel Create(int storyFrameId)
        {
            var oneCharaStoryModel = ModelsComposite.CharacterStoryModelManager.FindModel(storyFrameId);

            AppearListViewModel appearListViewModel = new AppearListViewModel();
            appearListViewModel.DisplayOrNoDisplayHeader = "登場する/しない";

            appearListViewModel.DataList = new ObservableCollection<AppearListViewItemModel>();

            foreach(var markableItem in ModelsComposite.CharacterManager.ModelCollection)
            {
                var charaItem = markableItem as CharacterModel;

                bool isAppeared = (oneCharaStoryModel.FindModel(charaItem.Id) == null)? false : true;

                appearListViewModel.DataList.Add(new AppearListViewItemModel(charaItem.Name, isAppeared, "登場する", "登場しない", charaItem));
            }

            return appearListViewModel;
        }
    }
}
