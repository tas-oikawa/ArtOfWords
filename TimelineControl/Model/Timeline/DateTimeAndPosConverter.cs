using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimelineControl.Model
{
    public class DateTimeAndPosConverter
    {
        double _minPos;
        double _maxPos;
        TimeRange _range;

        public DateTimeAndPosConverter(double minPos, double maxPos, TimeRange range)
        {
            _minPos = minPos;
            _maxPos = maxPos;
            _range = range;
        }

        public DateTime ConvertToDateTime(double pos)
        {
            double posRange = _maxPos - _minPos;
            double ratio = (pos - _minPos) / posRange;

            TimeSpan span = new TimeSpan((long)((_range.EndDateTime.Ticks - _range.StartDateTime.Ticks) * ratio));
            return _range.StartDateTime.Add(span);
        }

        public double ConvertToPos(DateTime time)
        {
            double posRange = _maxPos - _minPos;

            TimeSpan span = new TimeSpan(_range.EndDateTime.Ticks - _range.StartDateTime.Ticks);
            double ratio = ((double)(time.Ticks - _range.StartDateTime.Ticks) / span.Ticks);

            return _minPos + ((_maxPos - _minPos) * ratio);
        }
    }
}
