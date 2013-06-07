using ModernizedAlice.ShosenColorPicker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModernizedAlice.ShosenColorPicker
{
    /// <summary>
    /// Interaction logic for ColorButton.xaml
    /// </summary>
    public partial class ColorButton : UserControl
    {
        private ColorButtonModel _model = new ColorButtonModel();

        public static readonly DependencyProperty ColorProperty =
                                DependencyProperty.Register("Color", typeof(HsvColor), typeof(ColorButton));
        public HsvColor Color
        {
            get { return (HsvColor)GetValue(ColorProperty); }
            set 
            { 
                SetValue(ColorProperty, value);
                var rgb = HsvColor.ToRgb(value);
                _model.ColorBrush = new SolidColorBrush(rgb);
                _model.DefaultBorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(128, rgb.R, rgb.G, rgb.B));
            }
        }

        public bool IsSelected
        {
            get
            {
                return _model.IsSelected;
            }
            set
            {
                if (_model.IsSelected == value)
                {
                    return;
                }

                _model.IsSelected = value;

                if (value)
                {
                    if (OnImSelected != null)
                    {
                        OnImSelected(this, value);
                    }
                }
            }
        }
        
        public delegate void OnIsSelectedChangedHandler(object sender, bool selection);
        public event OnIsSelectedChangedHandler OnImSelected;

        public ColorButton()
        {
            _model = new ColorButtonModel();
            InitializeComponent();

            this.DataContext = _model;
        }

        private void _colorRect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSelected == true)
            {
                return;
            }
            IsSelected = !IsSelected;
        }

    }
}
