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

namespace FileSelector.Model
{
    /// <summary>
    /// 転送データ
    /// </summary>
    public class NewFilePlusTransferData
    {
        /// <summary>
        /// 転送対象の登場人物
        /// </summary>
        public List<CharacterModel> Characters { get; set; }

        /// <summary>
        /// 転送対象のアイテム
        /// </summary>
        public List<ItemModel> Items { get; set; }

        /// <summary>
        /// 転送対象の展開
        /// </summary>
        public List<StoryFrameModel> StoryFrames { get; set; }

        /// <summary>
        /// 転送対象のイベント
        /// </summary>
        public List<TimelineEventModel> Events { get; set; }

        /// <summary>
        /// 転送対象のタグ
        /// </summary>
        public List<SaveTagModel> Tags { get; set; }


        /// <summary>
        /// 転送対象の登場人物展開リレーション
        /// </summary>
        public List<CharacterStoryRelationModel> CharaStoryRelations { get; set; }


        /// <summary>
        /// 転送対象のアイテム展開リレーション
        /// </summary>
        public List<ItemStoryRelationModel> ItemStoryRelations { get; set; }

        /// <summary>
        /// 転送対象の場所
        /// </summary>
        public List<PlaceModel> Places { get; set; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public NewFilePlusTransferData()
        {
            Characters = new List<CharacterModel>();
            Items = new List<ItemModel>();
            StoryFrames = new List<StoryFrameModel>();
            Events = new List<TimelineEventModel>();
            Tags = new List<SaveTagModel>();
            CharaStoryRelations = new List<CharacterStoryRelationModel>();
            ItemStoryRelations = new List<ItemStoryRelationModel>();
            Places = new List<PlaceModel>();
        }
    }
}
