using FileSelector.Model;
using FileSelector.ViewModels.NewFilePlus;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Relation;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.Biz
{
    /// <summary>
    /// つよくてニューファイルBiz
    /// </summary>
    public static class NewFilePlusGenerator
    {
        #region ViewModelGetter
        /// <summary>
        /// NewFilePlusViewModelを作って返す
        /// </summary>
        /// <returns>NewFilePlusViewModel</returns>
        public static NewFilePlusViewModel GetNewFilePlusViewModel()
        {
            var viewModel = new NewFilePlusViewModel();

            viewModel.Characters = GetCharacterListItems();
            viewModel.Items = GetItemListItems();
            viewModel.Events = GetEventListItems();
            viewModel.StoryFrames = GetStoryFrameListItems();
            viewModel.DoTransferTags = true;

            return viewModel;
        }

        /// <summary>
        /// 登場人物のListを返す
        /// </summary>
        /// <returns></returns>
        private static List<CharacterTransferItemViewModel> GetCharacterListItems()
        {
            var retList = new List<CharacterTransferItemViewModel>();

            var models = ModelsComposite.CharacterManager.ModelCollection;

            foreach (var model in models)
            {
                retList.Add(new CharacterTransferItemViewModel(model as CharacterModel));
            }

            return retList;
        }

        /// <summary>
        /// アイテムのListを返す
        /// </summary>
        /// <returns></returns>
        private static List<ItemTransferItemViewModel> GetItemListItems()
        {
            var retList = new List<ItemTransferItemViewModel>();

            var models = ModelsComposite.ItemModelManager.ModelCollection;

            foreach (var model in models)
            {
                retList.Add(new ItemTransferItemViewModel(model as ItemModel));
            }

            return retList;
        }

        /// <summary>
        /// 展開のListを返す
        /// </summary>
        /// <returns></returns>
        private static List<StoryFrameTransferItemViewModel> GetStoryFrameListItems()
        {
            var retList = new List<StoryFrameTransferItemViewModel>();

            var models = ModelsComposite.StoryFrameModelManager.ModelCollection;

            foreach (var model in models)
            {
                retList.Add(new StoryFrameTransferItemViewModel(model as StoryFrameModel));
            }

            return retList;
        }

        /// <summary>
        /// イベントのListを返す
        /// </summary>
        /// <returns></returns>
        private static List<EventTransferItemViewModel> GetEventListItems()
        {
            var retList = new List<EventTransferItemViewModel>();

            var models = ModelsComposite.TimelineEventModelManager.ModelCollection;

            foreach (var model in models)
            {
                retList.Add(new EventTransferItemViewModel(model as TimelineEventModel));
            }

            return retList;
        }

        /// <summary>
        /// タグのListを返す
        /// </summary>
        /// <returns></returns>
        private static List<TagTransferItemViewModel> GetTagListItems()
        {
            var retList = new List<TagTransferItemViewModel>();

            var models = TagExpander.Expand(ModelsComposite.TagManager);

            foreach (var model in models)
            {
                retList.Add(new TagTransferItemViewModel(model as SaveTagModel));
            }

            return retList;
        }

        #endregion

        #region GetTransferDataFromViewModel

        /// <summary>
        /// ViewModelから転送データを取得する
        /// </summary>
        /// <param name="model">データソース</param>
        /// <returns>転送データ</returns>
        public static NewFilePlusTransferData GetTransferData(NewFilePlusViewModel model)
        {
            var transferData = new NewFilePlusTransferData();

            transferData.Characters = GetTransferCharacterData(model.Characters);
            transferData.Items = GetTransferItemData(model.Items);
            transferData.StoryFrames = GetTransferStoryFrameData(model.StoryFrames);
            transferData.Events = GetTransferEventData(model.Events);

            if (model.DoTransferTags)
            {
                transferData.Tags = GetTransferTagData(GetTagListItems());
            }

            transferData.Places = GetTransferPlaceData();

            transferData.CharaStoryRelations = 
                GetTransferCSRelationData(ModelsComposite.CharacterStoryModelManager.ModelCollection.ToList(),
                transferData.Characters,
                transferData.StoryFrames);

            transferData.ItemStoryRelations =
                GetTransferISRelationData(ModelsComposite.ItemStoryModelManager.ModelCollection.ToList(),
                transferData.Items,
                transferData.StoryFrames);

            RemoveInvalidDataFromCharacters(transferData.Characters, transferData.Tags);
            RemoveInvalidDataFromItem(transferData.Items, transferData.Tags);
            RemoveInvalidDataFromEvents(transferData.Events, transferData.Characters);
            RemoveInvalidDataFromStoryFrame(transferData.Characters, transferData.Tags);
            RemoveInvalidPlaces(transferData.Places, transferData.StoryFrames);

            return transferData;
        }

        /// <summary>
        /// 登場人物の転送データを取得する
        /// </summary>
        /// <param name="srcCharas">データソース</param>
        /// <returns>転送データ</returns>
        private static List<CharacterModel> GetTransferCharacterData(List<CharacterTransferItemViewModel> srcCharas)
        {
            return srcCharas.Where((e) => e.IsSelected).Select((e) => e.Source).ToList<CharacterModel>();
        }

        /// <summary>
        /// アイテム転送データを取得する
        /// </summary>
        /// <param name="srcItems">データソース</param>
        /// <returns>転送データ</returns>
        private static List<ItemModel> GetTransferItemData(List<ItemTransferItemViewModel> srcItems)
        {
            return srcItems.Where((e) => e.IsSelected).Select((e) => e.Source).ToList<ItemModel>();
        }

        /// <summary>
        /// 展開転送データを取得する
        /// </summary>
        /// <param name="srcStoryFrames">データソース</param>
        /// <returns>転送データ</returns>
        private static List<StoryFrameModel> GetTransferStoryFrameData(
            List<StoryFrameTransferItemViewModel> srcStoryFrames)
        {
            return  srcStoryFrames.Where((e) => (e.IsSelected)).Select((e) => e.Source).ToList<StoryFrameModel>();
        }

        /// <summary>
        /// イベント転送データを取得する
        /// </summary>
        /// <param name="srcEvents">データソース</param>
        /// <returns>転送データ</returns>
        private static List<TimelineEventModel> GetTransferEventData(
            List<EventTransferItemViewModel> srcEvents)
        {
            return srcEvents.Where((e) => (e.IsSelected)).Select((e) => e.Source).ToList<TimelineEventModel>();
        }

        /// <summary>
        /// タグ転送データを取得する
        /// </summary>
        /// <param name="srcEvents">データソース</param>
        /// <returns>転送データ</returns>
        private static List<SaveTagModel> GetTransferTagData(
            List<TagTransferItemViewModel> srcEvents)
        {
            return srcEvents.Where((e) => (e.IsSelected)).Select((e) => e.Source).ToList<SaveTagModel>();
        }

        /// <summary>
        /// 場所データを取得する
        /// </summary>
        /// <returns>転送データ</returns>
        private static List<PlaceModel> GetTransferPlaceData()
        {
            var places = new List<PlaceModel>();

            foreach(var model in ModelsComposite.PlaceModelManager.ModelCollection)
            {
                places.Add(model as PlaceModel);
            }
            
            return places;
        }

        /// <summary>
        /// 登場人物展開のリレーションを取得する
        /// </summary>
        /// <param name="srcCharaStoryRelation">データソースの登場人物展開</param>
        /// <param name="transferCharacters">転送する登場人物</param>
        /// <param name="transferStoryFrames">転送する展開</param>
        /// <returns>転送データ</returns>
        private static List<CharacterStoryRelationModel> GetTransferCSRelationData(
            List<OneStoryFrameCharacterStoryRelationModelManager> srcCharaStoryRelation,
            List<CharacterModel> transferCharacters,
            List<StoryFrameModel> transferStoryFrames)
        {
            var csrModels = new List<CharacterStoryRelationModel>();

            foreach (var oneSFC in srcCharaStoryRelation)
            {
                var story = transferStoryFrames.FirstOrDefault((e) => e.Id == oneSFC.StoryFrameId);

                if (story == null)
                {
                    continue;
                }

                foreach (var csr in oneSFC.ModelCollection)
                {
                    if (transferCharacters.Exists((e) => e.Id == csr.CharacterId))
                    {
                        csrModels.Add(csr);
                    }
                }
            }

            return csrModels;
        }

        /// <summary>
        /// アイテム展開のリレーションを取得する
        /// </summary>
        /// <param name="srcItemStoryRelation">データソースのアイテム展開</param>
        /// <param name="transferCharacters">転送する登場人物</param>
        /// <param name="transferStoryFrames">転送する展開</param>
        /// <returns>転送データ</returns>
        private static List<ItemStoryRelationModel> GetTransferISRelationData(
            List<OneStoryFrameItemStoryRelationModelManager> srcItemStoryRelation,
            List<ItemModel> transferItems,
            List<StoryFrameModel> transferStoryFrames)
        {
            var isrModels = new List<ItemStoryRelationModel>();

            foreach (var oneSFC in srcItemStoryRelation)
            {
                var story = transferStoryFrames.FirstOrDefault((e) => e.Id == oneSFC.StoryFrameId);

                if (story == null)
                {
                    continue;
                }

                foreach (var csr in oneSFC.ModelCollection)
                {
                    if (transferItems.Exists((e) => e.Id == csr.ItemId))
                    {
                        isrModels.Add(csr);
                    }
                }
            }

            return isrModels;
        }

        /// <summary>
        /// イベントデータから無効なデータを削除する
        /// </summary>
        /// <param name="srcEvents">データソースのイベント</param>
        /// <param name="transferCharacters">転送対象の登場人物</param>
        private static void RemoveInvalidDataFromEvents(
            List<TimelineEventModel> srcEvents,
            List<CharacterModel> transferCharacters)
        {
            var deleteTarget = new List<TimelineEventModel>();

            foreach (var ev in srcEvents)
            {
                ev.Participants.RemoveAll((e) => (transferCharacters.Exists((f) => f.Id == e) == false));
            }

            srcEvents.RemoveAll((e) => e.Participants.Any() == false);
        }

        /// <summary>
        /// 登場人物データから無効なデータを削除する
        /// </summary>
        /// <param name="srcCharaters">データソースの登場人物</param>
        /// <param name="transferTags">転送対象のタグ</param>
        private static void RemoveInvalidDataFromCharacters(
            List<CharacterModel> srcCharaters,
            List<SaveTagModel> transferTags)
        {
            foreach (var chara in srcCharaters)
            {
                var unexistsTags = chara.Tags.Where((e) => transferTags.Exists((f) => f.Id == e) == false).ToList();

                foreach (var delTag in unexistsTags)
                {
                    chara.Tags.Remove(delTag);
                }
            }
        }

        /// <summary>
        /// 展開データから無効なデータを削除する
        /// </summary>
        /// <param name="srcCharacters">データソースの展開</param>
        /// <param name="transferTags">転送対象のタグ</param>
        private static void RemoveInvalidDataFromStoryFrame(
            List<CharacterModel> srcCharacters,
            List<SaveTagModel> transferTags)
        {
            foreach (var storyFrame in srcCharacters)
            {
                var unexistsTags = storyFrame.Tags.Where((e) => transferTags.Exists((f) => f.Id == e) == false).ToList();

                foreach (var delTag in unexistsTags)
                {
                    storyFrame.Tags.Remove(delTag);
                }
            }
        }

        /// <summary>
        /// アイテムデータから無効なデータを削除する
        /// </summary>
        /// <param name="srcCharacters">データソースのアイテム</param>
        /// <param name="transferTags">転送対象のタグ</param>
        private static void RemoveInvalidDataFromItem(
            List<ItemModel> srcItems,
            List<SaveTagModel> transferTags)
        {
            foreach (var item in srcItems)
            {
                var unexistsTags = item.Tags.Where((e) => transferTags.Exists((f) => f.Id == e) == false).ToList();

                foreach (var delTag in unexistsTags)
                {
                    item.Tags.Remove(delTag);
                }
            }
        }


        /// <summary>
        /// 場所データから無効なデータを削除する
        /// </summary>
        /// <param name="srcPlaces">データソースの場所</param>
        /// <param name="transferStoryFrame">転送対象の展開</param>
        private static void RemoveInvalidPlaces(
            List<PlaceModel> srcPlaces,
            List<StoryFrameModel> transferStoryFrame)
        {
            var unexistSrcPlaces = srcPlaces.Where((e) => transferStoryFrame.Exists((f) => f.PlaceId == e.Id) == false).ToList();

            foreach (var delPlc in unexistSrcPlaces)
            {
                srcPlaces.Remove(delPlc);
            }
        }

        #endregion
    }
}
