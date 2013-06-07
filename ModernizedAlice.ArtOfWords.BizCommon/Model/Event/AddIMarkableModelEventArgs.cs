using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Event
{
    public class AddIMarkableModelEventArgs : EventArgs
    {
        public IMarkable Markable;
        public AddIMarkableModelEventArgs(IMarkable markable)
        {
            Markable = markable;
        }
    }
}
