using CommonControls.Model;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StoryFrameBuildControl.Model
{
   public  class ItemStoryAppearListViewModelFactory
   {
       public static AppearListViewModel Create(int storyFrameId)
       {
           var oneItemStoryModel = ModelsComposite.ItemStoryModelManager.FindModel(storyFrameId);

           AppearListViewModel appearListViewModel = new AppearListViewModel();
           appearListViewModel.DisplayOrNoDisplayHeader = "登場する/しない";

           appearListViewModel.DataList = new ObservableCollection<AppearListViewItemModel>();

           foreach (var markableItem in ModelsComposite.ItemModelManager.ModelCollection)
           {
               var itemItem = markableItem as ItemModel;

               bool isAppeared = (oneItemStoryModel.FindModel(itemItem.Id) == null) ? false : true;

               appearListViewModel.DataList.Add(new AppearListViewItemModel(itemItem.Name, isAppeared, "登場する", "登場しない", itemItem));
           }

           return appearListViewModel;
       }
    }
}
