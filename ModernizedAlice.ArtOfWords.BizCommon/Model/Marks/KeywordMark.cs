using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class KeywordMark : Mark
    {
        public string keyword { set; get; }

        public override object Clone()
        {
            return new KeywordMark()
            {
                Brush = base.Brush,
                HeadCharIndex = base.HeadCharIndex,
                keyword = keyword,
                Parent = base.Parent,
                TailCharIndex = base.TailCharIndex
            };
        }
    }
}
