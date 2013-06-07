using CommonControls.Model;
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

namespace CommonControls
{
    /// <summary>
    /// TextBoxColorPicker.xaml の相互作用ロジック
    /// </summary>
    public partial class TextBoxColorPicker : UserControl
    {
        private TextBoxColorPickerViewModel _model;

        public Color SelectingFontColor
        {
            set
            {
                _model.SelectingFontColor = value;
            }
            get
            {
                return _model.SelectingFontColor;
            }
        }
        public Color SelectingBackgroundColor
        {
            set
            {
                _model.SelectingBackgroundColor = value;
            }
            get
            {
                return _model.SelectingBackgroundColor;
            }
        }
        public FontFamily SelectingFontFamily
        {
            set
            {
                _model.SelectingFontFamily = value;
            }
            get
            {
                return _model.SelectingFontFamily;
            }
        }
        public double SelectingFontSize
        {
            set
            {
                _model.SelectingFontSize = value;
            }
            get
            {
                return _model.SelectingFontSize;
            }
        }

        public TextBoxColorPicker()
        {
            _model = new TextBoxColorPickerViewModel();
            InitializeComponent();

            this.DataContext = _model;

            _colorPicker.SelectColorChanged += _colorPicker_SelectColorChanged;
        }

        void _colorPicker_SelectColorChanged(ModernizedAlice.ShosenColorPicker.Model.HsvColor color)
        {
            var selColor = _colorPicker.GetSelectingColor();
            if (selColor == null)
            {
                return;
            }

            _model.SelectingColor = selColor.GetValueOrDefault();
        }

        private void _fontColorSelectButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SelectingFontColor = _model.SelectingColor;
        }

        private void _backgroundColorSelectButton_Click(object sender, RoutedEventArgs e)
        {
            _model.SelectingBackgroundColor = _model.SelectingColor;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            _colorPicker.Draw();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            _model.SelectingFontColor = (btn.Foreground as SolidColorBrush).Color;
            _model.SelectingBackgroundColor = (btn.Background as SolidColorBrush).Color;
        }
    }
}
