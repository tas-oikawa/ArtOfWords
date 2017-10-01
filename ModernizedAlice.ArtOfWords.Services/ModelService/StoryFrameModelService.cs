using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.Services.ModelService
{
    /// <summary>
    /// 展開のモデルを扱うService
    /// </summary>
    public class StoryFrameModelService
    {
        /// <summary>
        /// 新しい展開を追加する
        /// </summary>
        /// <returns>作成した展開</returns>
        public StoryFrameModel AddNewStoryFrame()
        {
            var manager = ModelsComposite.StoryFrameModelManager;

            var newModel = manager.GetNewModel();

            var placeService = new PlaceModelService();

            // 固有の場所を取得する
            var place = placeService.AddNewPlace(); 

            newModel.PlaceId = place.Id;

            return AddStoryFrame(newModel);
        }
        /// <summary>
        /// 新しい展開を追加する
        /// </summary>
        /// <param name="storyModel">追加する展開</param>
        /// <returns>作成した展開</returns>
        public StoryFrameModel AddStoryFrame(StoryFrameModel storyModel)
        {
            var manager = ModelsComposite.StoryFrameModelManager;
            
            manager.AddModel(storyModel);

            ModelsComposite.CharacterStoryModelManager.AddStoryFrameModel(storyModel.Id);
            ModelsComposite.ItemStoryModelManager.AddStoryFrameModel(storyModel.Id);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnAddIMarkable(this, new AddIMarkableModelEventArgs(storyModel));

            return storyModel;
        }

        /// <summary>
        /// 展開を削除する
        /// </summary>
        /// <param name="rmModel">展開する展開</param>
        public void RemoveStoryFrame(StoryFrameModel rmModel)
        {
            var strManager = ModelsComposite.StoryFrameModelManager;
            strManager.RemoveModel(rmModel);

            var csmManager = ModelsComposite.CharacterStoryModelManager;
            csmManager.RemoveModel(rmModel);

            var isManager = ModelsComposite.ItemStoryModelManager;
            isManager.RemoveModel(rmModel);

            var plcManager = ModelsComposite.PlaceModelManager;

            var findPlace = plcManager.FindPlaceModel(rmModel.PlaceId);
            if (findPlace != null)
            {
                plcManager.RemoveModel(findPlace);
            }

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnDeleteIMarkable(this, new DeleteIMarkableModelEventArgs(rmModel));
        }
    }
}
