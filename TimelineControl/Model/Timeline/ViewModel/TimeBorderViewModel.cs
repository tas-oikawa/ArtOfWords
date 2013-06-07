using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TimelineControl.Model
{
    public class TimeBorderViewModel
    {
        private DateTime _startDateTime;

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }
        private DateTime _endDateTime;

        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { _endDateTime = value; }
        }

        private TimelineAxis _sourceObject;

        public TimelineAxis SourceObject
        {
            get { return _sourceObject; }
            set { _sourceObject = value; }
        }

        private SolidColorBrush _myBrush;

        public SolidColorBrush MyBrush
        {
            get { return _myBrush; }
            set { _myBrush = value; }
        }

        public SolidColorBrush LightColorBrush
        {
            get
            {
                return new SolidColorBrush(ColorUtil.ExtremelyLighter(_myBrush.Color));
            }
        }

        public SolidColorBrush LittleLightColorBrush
        {
            get
            {
                return new SolidColorBrush(ColorUtil.Lighter(_myBrush.Color, 0.7));
            }
        }
    }
}
