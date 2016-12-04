using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.Services.ModelService
{
    /// <summary>
    /// アイテムのモデルを扱うService
    /// </summary>
    public class ItemModelService
    {
        /// <summary>
        /// 新しいアイテムを追加する
        /// </summary>
        /// <returns>作成したアイテム</returns>
        public ItemModel AddNewItem()
        {
            var manager = ModelsComposite.ItemModelManager;

            var newModel = manager.GetNewModel();
            return AddItem(newModel);
            
        }

        /// <summary>
        /// 新しいアイテムを追加する
        /// </summary>
        /// <returns>作成したアイテム</returns>
        public ItemModel AddItem(ItemModel addModel)
        {
            var manager = ModelsComposite.ItemModelManager;
            manager.AddModel(addModel);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnAddIMarkable(this, new AddIMarkableModelEventArgs(addModel));

            return addModel;
        }
    }
}
