using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model
{
    public interface ITagStickable
    {
        List<int> GetTagIds();
        void SetTagIds(List<int> stickTagList);
    }
}
