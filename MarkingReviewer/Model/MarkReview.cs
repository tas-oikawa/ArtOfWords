using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;

namespace Chokanbar.Model
{
    public class MarkReview
    {
        private Mark _mark;
        private String _document;

        public MarkReview(Mark mark, String document)
        {
            _mark = mark;
            _document = document;
        }

        public Brush BorderColor
        {
            get
            {
                return _mark.Brush;
            }
        }

        public string Text
        {
            get
            {
                if (_mark is StoryFrameMark)
                {
                    return (_mark.Parent as StoryFrameModel).Name;
                }
                return _document.Substring(_mark.HeadCharIndex, _mark.TailCharIndex - _mark.HeadCharIndex + 1 );
            }
        }

        public string GetKeywordText()
        {
            if (_mark is KeywordMark)
            {
                var keyMark = _mark as KeywordMark;
                return keyMark.keyword;
            }
            if (_mark is StoryFrameMark)
            {
                var keyMark = _mark as StoryFrameMark;
                return _document.Substring(_mark.HeadCharIndex, _mark.TailCharIndex - _mark.HeadCharIndex + 1);
            }

            return Text;
        }

        public int HeadIndex
        {
            get
            {
                return _mark.HeadCharIndex;
            }
        }
    }
}
