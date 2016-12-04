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
    }
}
