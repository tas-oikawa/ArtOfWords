using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Tag
{
    public class BaseTag : Tag
    {
        public override bool IsBase()
        {
            return true;
        }

        public BaseTag() : base(0)
        {
            Name = "タグ";
        }
    }
}
