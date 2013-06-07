using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using System.Xml.Serialization;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class Mark : ICloneable
    {
        public delegate void MarkChangedEvent(Object sender, MarkChangedEventArgs e);
        public event MarkChangedEvent MarkChanged;

        [XmlIgnoreAttribute]
        public IMarkable Parent { set; get; }
        
        private int _headCharIndex;
        public int HeadCharIndex 
        {
            set
            {
                if (_headCharIndex != value)
                {
                    _headCharIndex = value;
                    if (MarkChanged != null)
                    {
                        MarkChanged(this, new MarkChangedEventArgs() { MarkChangeKind = MarkChangeKind.IndexChanged });
                    }
                }
            }
            get
            {
                return _headCharIndex;
            }
        }

        private int _tailCharIndex;
        public int TailCharIndex
        {
            set
            {
                if (_tailCharIndex != value)
                {
                    _tailCharIndex = value;
                    if (MarkChanged != null)
                    {
                        MarkChanged(this, new MarkChangedEventArgs() { MarkChangeKind = MarkChangeKind.IndexChanged });
                    }
                }
            }
            get
            {
                return _tailCharIndex;
            }
        }

        public Brush Brush { set; get; }

        public Mark()
        {
        }
        public Mark(Mark mark)
        {
            HeadCharIndex = mark.HeadCharIndex;
            TailCharIndex = mark.TailCharIndex;
            Brush = mark.Brush.CloneCurrentValue();
        }

        public virtual String GetRemark()
        {
            return "";
        }

        public virtual object Clone()
        {
            return null;
        }

    }
}
