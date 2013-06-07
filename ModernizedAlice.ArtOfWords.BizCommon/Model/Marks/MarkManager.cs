using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class MarkManager
    {
        private List<Mark> _markList;
        private MarkCache _markCache;

        public delegate void MarkChangedEvent(Object sender, MarkChangedEventArgs e);
        public event MarkChangedEvent MarkChanged;

        public MarkManager()
        {
            _markList = new List<Mark>();
            _markCache = new MarkCache();
        }

        public void AddMark(Mark mark)
        {
            mark.MarkChanged += MarkChangedEventHandler;

            _markList.Add(mark);

            if (MarkChanged != null)
            {
                MarkChanged(this, new MarkChangedEventArgs() { MarkChangeKind = MarkChangeKind.Add });
            }
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }

        public void DeleteMark(Mark mark)
        {
            _markList.Remove(mark);

            if (MarkChanged != null)
            {
                MarkChanged(this, new MarkChangedEventArgs() { MarkChangeKind = MarkChangeKind.Delete });
            }
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }

        public void MarkChangedEventHandler(object obj, MarkChangedEventArgs arg)
        {
            if (MarkChanged != null)
            {
                MarkChanged(this, arg);
            }
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }

        public Mark GetPriorityMark(int index)
        {
            return _markCache.GetHeadMark(index);
        }

        /// <summary>
        /// 先頭と末尾によってキャッシュを作成します。これを作成することで検索速度が上がります
        /// </summary>
        /// <param name="headIndex"></param>
        /// <param name="tailIndex"></param>
        public void FocusOnRange(int headIndex, int tailIndex, MarkKindEnums kind)
        {
            var regionMarks = GetMarks(headIndex, tailIndex, kind);

            _markCache.GenerateCache(regionMarks, headIndex, tailIndex);
        }


        #region Utils

        private MarkKindEnums GetMarkKind(Mark mark)
        {
            if (mark.GetType() == typeof(CharacterMark))
            {
                return MarkKindEnums.Character;
            }
            else if (mark.GetType() == typeof(StoryFrameMark))
            {
                return MarkKindEnums.StoryFrame;
            }

            return MarkKindEnums.Character;
        }

        private bool isTargetMark(int headIndex, int tailIndex, Mark mark, MarkKindEnums kind)
        {
            if (GetMarkKind(mark) != kind)
            {
                return false;
            }

            if (headIndex > mark.TailCharIndex)
            {
                return false;
            }

            if (tailIndex < mark.HeadCharIndex)
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Mark> GetMarks(int headIndex, int tailIndex, MarkKindEnums markKind)
        {
            var marks = from mrk in _markList
                        where isTargetMark(headIndex, tailIndex, mrk, markKind)
                        select mrk;

            return marks;
        }

        public IEnumerable<Mark> GetMarks(IMarkable markable)
        {
            var marks = from mrk in _markList
                        where mrk.Parent == markable
                        select mrk;

            return marks;
        }

        public IEnumerable<Mark> GetMarks()
        {
            return _markList; 
        }


        #endregion

    }
}
