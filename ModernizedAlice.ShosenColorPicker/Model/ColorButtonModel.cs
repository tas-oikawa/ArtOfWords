using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ModernizedAlice.ShosenColorPicker.Model
{
    public class ColorButtonModel : INotifyPropertyChanged
    {
        private Brush _colorBrush;
        public Brush ColorBrush
        {
            get { return _colorBrush; }
            set
            {
                _colorBrush = value;
                OnPropertyChanged("ColorBrush");
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
                OnPropertyChanged("BorderBrush");
                OnPropertyChanged("BorderThickness");
            }
        }

        public Brush DefaultBorderBrush;

        public Brush BorderBrush
        {
            get
            {
                if (IsSelected)
                {
                    return Brushes.Yellow;
                }

                return DefaultBorderBrush;
            }
        }

        public Thickness BorderThickness
        {
            get
            {
                if (IsSelected)
                {
                    return new Thickness(2);
                }

                return new Thickness(1);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
