using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Event
{
    public class ReplaceWordEventArgs : EventArgs
    {
        public bool replaceAll;
        public string fromStr;
        public string toStr;

        public int headIndex;
        public int tailIndex;
    }
}
