using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TimelineControl.Model
{
    public class EventBorderViewModel
    {
        public EventModel Parent;

        private SolidColorBrush _myBrush;

        public SolidColorBrush MyBrush
        {
            get { return _myBrush; }
            set { _myBrush = value; }
        }

        public Color LightColor
        {
            get
            {
                return ColorUtil.Lighter(_myBrush.Color);
            }
        }

        public SolidColorBrush LightColorBrush
        {
            get
            {
                return new SolidColorBrush(ColorUtil.Lighter(_myBrush.Color));
            }
        }

        public SolidColorBrush LittleLightColorBrush
        {
            get
            {
                return new SolidColorBrush(ColorUtil.Lighter(_myBrush.Color));
            }
        }

        private String _title;

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }


    }
}
