using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks.MarkManagementStrategy;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class MarkFactory
    {
        public static bool AddMark(string text, int index, IMarkable targetMark)
        {
            if (targetMark.GetType() == typeof(CharacterModel))
            {
                CharacterMarkManagementStrategy.AddMark(text, index, targetMark);
                return true;
            }
            else if (targetMark.GetType() == typeof(StoryFrameModel))
            {
                StoryFrameMarkManagementStrategy.AddMark(text, index, targetMark);
                return true;
            }

            return false;
        }
    }
}
