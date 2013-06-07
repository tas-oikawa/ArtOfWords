using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class StoryFrameMark : Mark
    {
        public int MarkId { set; get; }

        public override object Clone()
        {
            return new StoryFrameMark()
            {
                Brush = base.Brush,
                HeadCharIndex = base.HeadCharIndex,
                MarkId = MarkId,
                Parent = base.Parent,
                TailCharIndex = base.TailCharIndex
            };
        }
    }
}
