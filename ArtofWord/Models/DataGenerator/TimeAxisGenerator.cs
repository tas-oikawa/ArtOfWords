using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using TimelineControl.Model;

namespace ArtOfWords.Models.DataGenerator
{
    /// <summary>
    /// タイムラインの縦軸を作るクラス
    /// </summary>
    public class TimeAxisGenerator
    {
        /// <summary>
        /// StoryFrameの軸を作る
        /// </summary>
        private static TimelineAxis GenerateAxisFromStory()
        {
            return new TimelineAxis()
            {
                Id = CommonConstants.STORY_FRAME_EVENT_ID,
                HeaderName = "シーン",
                Width = 140,
                IsDisplayed = true,
                IsUnbound = true,
                DrawBrush = Brushes.Black,
                SourceObject = new StoryFrameAxisModel()
            };
        }

        /// <summary>
        /// 登場人物の軸を作る
        /// </summary>
        private static TimelineAxis GenerateAxisFromChara(CharacterModel chara)
        {
            return new TimelineAxis()
            {
                Id = chara.Id,
                HeaderName = chara.Symbol,
                Width = 140,
                IsDisplayed = true,
                IsUnbound = false,
                DrawBrush = (SolidColorBrush)chara.ColorBrush,
                SourceObject = chara
            };
        }

        /// <summary>
        /// 全体通しての軸を作る
        /// </summary>
        /// <returns>軸のリスト</returns>
        public static List<TimelineAxis> Generate()
        {
            List<TimelineAxis> axisList = new List<TimelineAxis>();

            // まずシーンの軸を設定
            axisList.Add(GenerateAxisFromStory());
            
            // 登場人物からAxisを作成
            foreach (var iMarkable in ModelsComposite.CharacterManager.ModelCollection)
            {
                var chara = iMarkable as CharacterModel;

                axisList.Add(GenerateAxisFromChara(chara));
            }

            return axisList;
        }
    }
}
