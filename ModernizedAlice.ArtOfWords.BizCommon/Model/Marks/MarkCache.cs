using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Marks
{
    public class MarkCache
    {
        private Dictionary<int, List<Mark>> _cache;

        public MarkCache()
        {
        }

        private List<Mark> GetMarkListOrGenerateIfNot(int index)
        {
            if (_cache == null)
            {
                return null;
            }

            if (_cache.ContainsKey(index))
            {
                return _cache[index];
            }
            else
            {
                var newMarkList = new List<Mark>();

                _cache.Add(index, newMarkList);

                return newMarkList;
            }
        }

        public void GenerateCache(IEnumerable<Mark> regionMarks, int headIndex, int tailIndex)
        {
            _cache = new Dictionary<int, List<Mark>>();

            foreach (var mark in regionMarks)
            {
                int top = (mark.HeadCharIndex > headIndex) ? mark.HeadCharIndex : headIndex;
                int bottom = (mark.TailCharIndex > tailIndex) ? tailIndex : mark.TailCharIndex;

                for (int cur = top; cur <= bottom; ++cur)
                {
                    var list = GetMarkListOrGenerateIfNot(cur);
                    list.Add(mark);
                }
            }
        }

        public Mark GetHeadMark(int index)
        {
            if (_cache == null)
            {
                return null;
            }

            if(_cache.ContainsKey(index))
            {
                return (_cache[index])[0];
            }

            return null;
        }
    }
}
