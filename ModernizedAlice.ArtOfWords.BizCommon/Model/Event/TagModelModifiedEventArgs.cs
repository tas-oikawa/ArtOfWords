using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Event
{
    public enum TagModelModifiedKind
    {
        Add,
        Modified,
        Deleted,
    }

    public class TagModelModifiedEventArgs
    {
        public TagModelModifiedKind Kind;
        public TagModel ModifiedTag;
    }
}
