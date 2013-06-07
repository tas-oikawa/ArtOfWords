using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Event
{
    public class MoveDocumentIndexEventArgs : EventArgs
    {
        public int headIndex;

        public string targetText;
    }
}
