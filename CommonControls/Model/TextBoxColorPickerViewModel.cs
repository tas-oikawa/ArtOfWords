using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CommonControls.Model
{
    public class TextBoxColorPickerViewModel : INotifyPropertyChanged
    {
        private Color _selectingColor;
        private Color _selectingFontColor;
        private Color _selectingBackgroundColor;
        private FontFamily _selectingFontFamily;
        private double _selectingFontSize;

        public TextBoxColorPickerViewModel()
        {
            _selectingColor = Colors.White;
        }

        public Color SelectingColor
        {
            get { return _selectingColor; }
            set
            {
                if (_selectingColor != value)
                {
                    _selectingColor = value;
                    OnPropertyChanged("SelectingColor");
                    OnPropertyChanged("SelectingColorBrush");
                    OnPropertyChanged("RValue");
                    OnPropertyChanged("GValue");
                    OnPropertyChanged("BValue");
                }
            }
        }

        public Brush SelectingColorBrush
        {
            get { return new SolidColorBrush(SelectingColor); }
        }

        private bool TryParseAsRGBColor(string str, out int refValue)
        {
            if (int.TryParse(str, out refValue) == false)
            {
                return false;
            }

            if (refValue < 0 || refValue > 255)
            {
                return false;
            }

            return true;
        }

        public string RValue
        {
            set
            {
                int refValue;
                if (TryParseAsRGBColor(value, out refValue) == false)
                {
                    return;
                }

                SelectingColor = Color.FromRgb((byte)refValue, SelectingColor.G, SelectingColor.B);
            }
            get
            {
                return SelectingColor.R.ToString();
            }
        }

        public string GValue
        {
            set
            {
                int refValue;
                if (TryParseAsRGBColor(value, out refValue) == false)
                {
                    return;
                }

                SelectingColor = Color.FromRgb(SelectingColor.R, (byte)refValue, SelectingColor.B);
            }
            get
            {
                return SelectingColor.G.ToString();
            }
        }

        public string BValue
        {
            set
            {
                int refValue;
                if (TryParseAsRGBColor(value, out refValue) == false)
                {
                    return;
                }

                SelectingColor = Color.FromRgb(SelectingColor.R,  SelectingColor.G, (byte)refValue);
            }
            get
            {
                return SelectingColor.B.ToString();
            }
        }


        public Color SelectingFontColor
        {
            get { return _selectingFontColor; }
            set
            {
                if (_selectingFontColor != value)
                {
                    _selectingFontColor = value;
                    OnPropertyChanged("SelectingFontColor");
                    OnPropertyChanged("SelectingFontColorBrush");
                }
            }
        }


        public Color SelectingBackgroundColor
        {
            get { return _selectingBackgroundColor; }
            set
            {
                if (_selectingBackgroundColor != value)
                {
                    _selectingBackgroundColor = value;
                    OnPropertyChanged("SelectingBackgroundColor");
                    OnPropertyChanged("SelectingBackGroundColorBrush");
                }
            }
        }


        public FontFamily SelectingFontFamily
        {
            get { return _selectingFontFamily; }
            set
            {
                if (_selectingFontFamily != value)
                {
                    _selectingFontFamily = value;
                    OnPropertyChanged("SelectingFontFamily");
                }
            }
        }

        public double SelectingFontSize
        {
            get { return _selectingFontSize; }
            set
            {
                if (_selectingFontSize != value)
                {
                    _selectingFontSize = value;
                    OnPropertyChanged("SelectingFontSize");
                }
            }
        }

        public Brush SelectingFontColorBrush
        {
            get
            {
                return new SolidColorBrush(_selectingFontColor);
            }
        }

        public Brush SelectingBackGroundColorBrush
        {
            get
            {
                return new SolidColorBrush(_selectingBackgroundColor);
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
    }
}
