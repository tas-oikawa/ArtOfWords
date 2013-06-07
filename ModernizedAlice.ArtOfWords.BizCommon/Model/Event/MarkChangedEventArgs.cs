using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Event
{
    public enum MarkChangeKind
    {
        Add,
        Delete,
        IndexChanged,
    }

    public class MarkChangedEventArgs : EventArgs
    {
        public MarkChangeKind MarkChangeKind;
    }
}
