using ArtOfWords.DataGenerator.Model;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using TimelineControl.Model;

namespace ArtOfWords.DataGenerator
{
    public class TimeAxisGenerator
    {

        private static TimelineAxis GenerateAxisFromStory()
        {
            return new TimelineAxis()
            {
                Id = CommonDefinitions.StoryFrameEventId,
                HeaderName = "シーン",
                Width = 140,
                IsDisplayed = true,
                IsUnbound = true,
                DrawBrush = Brushes.Black,
                SourceObject = new StoryFrameAxisModel()
            };
        }

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
