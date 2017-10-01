using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.Services.ModelService
{
    /// <summary>
    /// 登場人物のモデルを扱うService
    /// </summary>
    public class CharacterModelService
    {
        /// <summary>
        /// 新しい登場人物を追加する
        /// </summary>
        /// <returns>作成した登場人物</returns>
        public CharacterModel AddNewCharacter()
        {
            var manager = ModelsComposite.CharacterManager;
            var newModel = manager.GetNewModel();

            return AddCharacter(newModel);
        }
        /// <summary>
        /// 新しい登場人物を追加する
        /// </summary>
        /// <param name="addModel">追加する登場人物</param>
        /// <returns>作成した登場人物</returns>
        public CharacterModel AddCharacter(CharacterModel addModel)
        {
            var manager = ModelsComposite.CharacterManager;
            
            manager.AddModel(addModel);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnAddIMarkable(this, new AddIMarkableModelEventArgs(addModel));

            return addModel;
        }

        /// <summary>
        /// 登場人物を削除する
        /// </summary>
        /// <param name="rmModel">削除するモデル</param>
        public void RemoveCharacter(CharacterModel rmModel)
        {
            var chManager = ModelsComposite.CharacterManager;
            chManager.RemoveModel(rmModel);

            var csrManager = ModelsComposite.CharacterStoryModelManager;
            csrManager.RemoveModel(rmModel);

            var tlManager = ModelsComposite.TimelineEventModelManager;
            tlManager.RemoveParticipants(rmModel);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnDeleteIMarkable(this, new DeleteIMarkableModelEventArgs(rmModel));
        }
    }
}
