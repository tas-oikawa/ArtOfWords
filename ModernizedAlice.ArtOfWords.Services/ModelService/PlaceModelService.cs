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
    /// 場所のサービス
    /// </summary>
    public class PlaceModelService
    {
        /// <summary>
        /// 新しい場所を追加する
        /// </summary>
        /// <returns>作成した場所</returns>
        public PlaceModel AddNewPlace()
        {
            var manager = ModelsComposite.PlaceModelManager;
            var newModel = manager.GetNewModel();

            return AddPlace(newModel);
        }
        /// <summary>
        /// 新しい場所を追加する
        /// </summary>
        /// <param name="addModel">追加する場所</param>
        /// <returns>作成した場所</returns>
        public PlaceModel AddPlace(PlaceModel addModel)
        {
            var manager = ModelsComposite.PlaceModelManager;

            manager.AddModel(addModel);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnAddIMarkable(this, new AddIMarkableModelEventArgs(addModel));

            return addModel;
        }
    }
}
