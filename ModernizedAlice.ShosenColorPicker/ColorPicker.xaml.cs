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
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private ColorPickerViewModel _model;

        public delegate void SelectColorChangedHandler(HsvColor color);
        public event SelectColorChangedHandler SelectColorChanged;

        public ColorPicker()
        {
            _model = new ColorPickerViewModel(this);

            InitializeComponent();

            this.DataContext = _model;
        }

        internal void OnSelectColorChanged(HsvColor color)
        {
            if (SelectColorChanged != null)
            {
                SelectColorChanged(color);
            }
        }

        public void Draw()
        {
            _model.Dye();
        }

        public Color? GetSelectingColor()
        {
            return _model.SelectingColor;
        }
    }
}
