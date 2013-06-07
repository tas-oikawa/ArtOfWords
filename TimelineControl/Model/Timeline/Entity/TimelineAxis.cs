using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace TimelineControl.Model
{
    public class TimelineAxis
    {
        private bool _isUnbound;

        private bool _isDisplayed;

        private int _id;

        private double _width;

        private string _headerName;

        private object _sourceObject;

        private SolidColorBrush _drawBrush;

        public bool IsUnbound
        {
            get { return _isUnbound; }
            set { _isUnbound = value; }
        }

        public bool IsDisplayed
        {
            get { return _isDisplayed; }
            set { _isDisplayed = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public string HeaderName
        {
            get { return _headerName; }
            set { _headerName = value; }
        }

        public object SourceObject
        {
            get { return _sourceObject; }
            set { _sourceObject = value; }
        }

        public SolidColorBrush DrawBrush
        {
            get { return _drawBrush; }
            set { _drawBrush = value; }
        }

        public Color LightColor
        {
            get
            {
                return ColorUtil.Lighter(_drawBrush.Color);
            }
        }

        public SolidColorBrush LightColorBrush
        {
            get
            {
                return new SolidColorBrush(ColorUtil.Lighter(_drawBrush.Color));
            }
        }

        public SolidColorBrush LittleLightColorBrush
        {
            get
            {
                return new SolidColorBrush(ColorUtil.Lighter(_drawBrush.Color, 0.7));
            }
        }
    }
}
