using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks.MarkManagementStrategy
{
    public class StoryFrameMarkManagementStrategy
    {
        private static bool IsRange(int index, Mark mark)
        {
            if (mark.HeadCharIndex >= index && mark.TailCharIndex <= index)
            {
                return true;
            }
            return false;
        }

        public static void AddStoryFrameMark(int head, int tail, StoryFrameModel model)
        {
            var storyFrameMark = new StoryFrameMark();
            storyFrameMark.Brush = model.MarkBrush;
            storyFrameMark.MarkId = model.Id;
            storyFrameMark.HeadCharIndex = head;
            storyFrameMark.TailCharIndex = tail;
            storyFrameMark.Parent = model;

            ModelsComposite.MarkManager.AddMark(storyFrameMark);
        }

        public static void AddStoryFrameMark(string text, int index, StoryFrameModel targetMark)
        {
            var marks = ModelsComposite.MarkManager.GetMarks(0, text.Length - 1 , MarkKindEnums.StoryFrame);

            int headIndex = index;
            int tailIndex = index;
         
            // 同じ開始インデックスがあれば回避
            foreach (var mark in marks)
            {
                if (mark.HeadCharIndex == index)
                {
                    return;
                }
            }

            foreach (var mark in marks)
            {
                // 同じのがあったら削除
                if (mark.Parent == targetMark)
                {
                    ModelsComposite.MarkManager.DeleteMark(mark);
                    break;
                }
            }

            AddStoryFrameMark(headIndex, tailIndex, targetMark);   
        }

        public static bool AddMark(string text, int index, IMarkable targetMark)
        {
            AddStoryFrameMark(text, index, targetMark as StoryFrameModel);
            return true;
        }
    }
}
