using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Event
{
    public class DeleteIMarkableModelEventArgs : EventArgs
    {
        public IMarkable Markable;
        public DeleteIMarkableModelEventArgs(IMarkable markable)
        {
            Markable = markable;
        }
    }
}
