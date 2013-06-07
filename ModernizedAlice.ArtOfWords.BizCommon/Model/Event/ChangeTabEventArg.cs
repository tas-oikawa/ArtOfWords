using ModernizedAlice.ArtOfWords.BizCommon.ControlUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Event
{
    public class ChangeTabEventArg : EventArgs
    {
        public MainTabKind ChangeTo;
        public object SubObject;
        public ChangeTabEventArg(MainTabKind changeTo, object markable)
        {
            ChangeTo = changeTo;
            SubObject = markable;
        }
    }
}
