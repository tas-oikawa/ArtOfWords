using FileSelector.Model;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Relation;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.Biz
{
    /// <summary>
    /// つよくてニューファイル転送
    /// </summary>
    public static class NewFilePlusTransferer
    {
        #region ModelsCompositeの揃え変え
        
        /// <summary>
        /// データを転送する
        /// </summary>
        /// <param name="transferData">転送データ</param>
        public static void Transfer(NewFilePlusTransferData transferData)
        {
            TransferCharacters(transferData.Characters);
            TransferPlaces(transferData.Places);
            TransferStoryFrames(transferData.StoryFrames);
            TransferItems(transferData.Items);
            TransferCharacterStoryRelation(transferData.CharaStoryRelations);
            TransferItemStoryRelation(transferData.ItemStoryRelations);
            TransferTags(transferData.Tags);
            TransferEvent(transferData.Events);
        }

        /// <summary>
        /// 登場人物の転送
        /// </summary>
        /// <param name="characters">登場人物</param>
        private static void TransferCharacters(List<CharacterModel> characters)
        {
            foreach (var model in characters)
            {
                ModelsComposite.CharacterManager.AddModel(model);
            }
        }

        /// <summary>
        /// アイテムの転送
        /// </summary>
        /// <param name="items">アイテム</param>
        private static void TransferItems(List<ItemModel> items)
        {
            foreach (var model in items)
            {
                ModelsComposite.ItemModelManager.AddModel(model);
            }
        }

        /// <summary>
        /// 展開の転送
        /// </summary>
        /// <param name="storyFrames">展開</param>
        private static void TransferStoryFrames(List<StoryFrameModel> storyFrames)
        {
            foreach (var model in storyFrames)
            {
                ModelsComposite.StoryFrameModelManager.AddModel(model);
                ModelsComposite.CharacterStoryModelManager.AddStoryFrameModel(model.Id);
                ModelsComposite.ItemStoryModelManager.AddStoryFrameModel(model.Id);
            }
        }

        /// <summary>
        /// 場所の転送
        /// </summary>
        /// <param name="places">場所</param>
        private static void TransferPlaces(List<PlaceModel> places)
        {
            foreach (var model in places)
            {
                ModelsComposite.PlaceModelManager.AddModel(model);
            }
        }

        /// <summary>
        /// 登場人物、展開リレーションの転送
        /// </summary>
        /// <param name="csRelations">登場人物、展開リレーション</param>
        private static void TransferCharacterStoryRelation(List<CharacterStoryRelationModel> csRelations)
        {
            foreach (var model in csRelations)
            {
                var oneMgr = ModelsComposite.CharacterStoryModelManager.FindModel(model.StoryFrameId);
                oneMgr.AddModel(model);
            }
        }

        /// <summary>
        /// アイテム、展開リレーションの転送
        /// </summary>
        /// <param name="isRelations">アイテム、展開リレーション</param>
        private static void TransferItemStoryRelation(List<ItemStoryRelationModel> isRelations)
        {
            foreach (var model in isRelations)
            {
                var oneMgr = ModelsComposite.ItemStoryModelManager.FindModel(model.StoryFrameId);
                oneMgr.AddModel(model);
            }
        }

        /// <summary>
        /// タグの転送
        /// </summary>
        /// <param name="tags">タグ</param>
        private static void TransferTags(List<SaveTagModel> tags)
        {
            TagExpander.Expand(tags, ModelsComposite.TagManager);
        }

        /// <summary>
        /// イベントの転送
        /// </summary>
        /// <param name="events">イベント</param>
        private static void TransferEvent(List<TimelineEventModel> events)
        {
            foreach (var model in events)
            {
                ModelsComposite.TimelineEventModelManager.AddModel(model);
            }

        }
        #endregion
    }
}
