using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ModernizedAlice.ShosenColorPicker.Model
{
    public class ColorPickerViewModel : INotifyPropertyChanged
    {
        private ColorPicker _view;

        private double _brightness;
        public double Brightness
        {
            get
            {
                return _brightness;
            }
            set
            {
                _brightness = value;
                ChangeBrightness();
                OnPropertyChanged("Brightness");
            }
        }

        private double _saturationSpan;
        public double SaturationSpan
        {
            get { return _saturationSpan; }
            set 
            {
                _saturationSpan = value;
                OnPropertyChanged("SaturationSpan");
            }
        }

        private double _hueSpan;
        public double HueSpan
        {
            get { return _hueSpan; }
            set
            {
                _hueSpan = value;
                OnPropertyChanged("HueSpan");
            }
        }

        private ColorButton _selectingColorButton;

        private IEnumerable<ColorButton> _colorButtons;

        public ColorPickerViewModel(ColorPicker view)
        {
            _view = view;

            HueSpan = 36;
            SaturationSpan = 10;
            Brightness = 1;
        }

        public void SelectColorChangedHandler(object sender, bool selection)
        {
            foreach (var btn in _colorButtons)
            {
                if (btn != sender)
                {
                    btn.IsSelected = false;
                }
            }

            _selectingColorButton = sender as ColorButton;

            _view.OnSelectColorChanged(_selectingColorButton.Color);
        }

        public Color? SelectingColor
        {
            get
            {
                if (_selectingColorButton != null)
                {
                    return HsvColor.ToRgb(_selectingColorButton.Color);
                }
                return null;
            }
        }

        private void ResetEvent()
        {
            if (_colorButtons == null)
            {
                return;
            }

            foreach (var btn in _colorButtons)
            {
                btn.OnImSelected -= SelectColorChangedHandler;
            }
        }

        private void SetEvent()
        {
            if (_colorButtons == null)
            {
                return;
            }

            foreach (var btn in _colorButtons)
            {
                btn.OnImSelected += SelectColorChangedHandler;
            }
        }

        private void ChangeBrightness()
        {
            if (_colorButtons == null)
            {
                return;
            }

            foreach (var btn in _colorButtons)
            {
                btn.Color = new HsvColor(btn.Color.H, btn.Color.S, (float)Brightness);
            }

            if (_selectingColorButton != null)
            {
                _view.OnSelectColorChanged(_selectingColorButton.Color);
            }
        }

        public void Dye()
        {
            CanvasDyer dyer = new CanvasDyer();
            dyer.HueSpan = this.HueSpan;
            dyer.SaturationSpan = this.SaturationSpan;
            dyer.Brightness = this.Brightness;
            
            ResetEvent();

            _colorButtons = dyer.Dye(_view.DrawCanvas);
            SetEvent();
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
