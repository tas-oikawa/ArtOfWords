using CommonControls.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CommonControls.Model
{
    public class AppearListViewItemModel : INotifyPropertyChanged
    {
        public object ParentObject { set; get; }

        private string _displayHeader;
        public string DisplayHeader
        {
            get
            {
                return _displayHeader;
            }
        }
        private string _noDisplayHeader;
        public string NoDisplayHeader
        {
            get
            {
                return _noDisplayHeader;
            }
        }


        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public bool IsAppearedChanged = false;

        private bool _isAppeared;
        public bool IsAppeared
        {
            set
            {
                if (value != _isAppeared)
                {
                    _isAppeared = value;
                    IsAppearedChanged = true;
                    OnPropertyChanged("IsAppeared");
                }
            }
            get
            {
                return _isAppeared;
            }
        }

        public SolidColorBrush LightColorBrush
        {
            get
            {
                return new SolidColorBrush(ColorUtil.Lighter(BackgroundBrush.Color));
            }
        }


        public Color MyColor
        {
            get
            {
                return ColorUtil.Lighter(BackgroundBrush.Color);
            }
        }

        private SolidColorBrush _backgroundBrush;

        public SolidColorBrush BackgroundBrush
        {
            get { return _backgroundBrush; }
            set
            {
                if (value != _backgroundBrush)
                {
                    _backgroundBrush = value;
                    OnPropertyChanged("BackgroundBrush");
                    OnPropertyChanged("MyColor");
                    OnPropertyChanged("LightColorBrush");
                }
            }
        }

        public AppearListViewItemModel(string name, bool isAppered, string displayStr, string noDisplayStr, object source)
        {
            _isAppeared = isAppered;
            _name = name;
            _displayHeader = displayStr;
            _noDisplayHeader = noDisplayStr;
            ParentObject = source;
        }

        public delegate void DoActionIfAppearedChanged();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
