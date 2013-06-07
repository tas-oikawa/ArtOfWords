using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ModernizedAlice.IPlugin.ModuleInterface
{
    public class TextStyle : INotifyPropertyChanged
    {
        private FontFamily _fontFamily;

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set
            {
                if (_fontFamily != value)
                {
                    _fontFamily = value;
                    OnPropertyChanged("FontFamily");
                }
            }
        }

        private double _fontSize;

        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged("FontSize");
                }
            }
        }
        private Brush _background;

        public Brush Background
        {
            get { return _background; }
            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged("Background");
                }
            }
        }
        private Brush _foreground;

        public Brush Foreground
        {
            get { return _foreground; }
            set
            {
                if (_foreground != value)
                {
                    _foreground = value;
                    OnPropertyChanged("Foreground");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void NotifyAll()
        {
            OnPropertyChanged("FontFamily");
            OnPropertyChanged("FontSize");
            OnPropertyChanged("Background");
            OnPropertyChanged("Foreground");
        }
    }
}
