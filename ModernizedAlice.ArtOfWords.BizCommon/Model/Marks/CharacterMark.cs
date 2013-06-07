using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class CharacterMark : Mark
    {
        public int CharacterId { set; get; }

        public override object Clone()
        {
            return new CharacterMark()
            {
                Brush = base.Brush,
                CharacterId = CharacterId,
                HeadCharIndex = base.HeadCharIndex,
                Parent = base.Parent,
                TailCharIndex = base.TailCharIndex
            };
        }
    }
}
