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
    /// GeneralColorPicker.xaml の相互作用ロジック
    /// </summary>
    public partial class GeneralColorPicker : UserControl
    {
        private GeneralColorPickerViewModel _model;

        public Color SelectingColor
        {
            set
            {
                _model.SelectingColor = value;
            }
            get
            {
                return _model.SelectingColor;
            }
        }

        public GeneralColorPicker()
        {
            _model = new GeneralColorPickerViewModel();
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

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            _colorPicker.Draw();
        }
    }
}
