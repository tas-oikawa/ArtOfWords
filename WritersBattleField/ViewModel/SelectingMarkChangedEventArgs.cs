using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model;

namespace WritersBattleField.ViewModel
{
    public class SelectingMarkChangedEventArgs : EventArgs
    {
        public IMarkable SelectingMark;
    }

}
