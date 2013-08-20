using CharacterBuildControll.Model;
using CommonControls;
using CommonControls.Util;
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

namespace CharacterBuildControll
{
    /// <summary>
    /// BaseControl.xaml の相互作用ロジック
    /// </summary>
    public partial class BaseControl : UserControl
    {
        public CharacterBuildViewModel Model
        {
            get
            {
                return this.DataContext as CharacterBuildViewModel;
            }
        }

        public BaseControl()
        {
            InitializeComponent();
        }

        private void colorbutton_Click(object sender, RoutedEventArgs e)
        {
            var userControl = new GeneralColorPicker();
            CommonLightBox lightBox = new CommonLightBox();
            lightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveCancel;
            lightBox.BindUIElement(userControl);
            lightBox.Owner = Application.Current.MainWindow;

            userControl.SelectingColor = ((SolidColorBrush)Model.SelectingModel.ColorBrush).Color;

            if (ShowDialogManager.ShowDialog(lightBox) == true)
            {
                Model.SelectingModel.ColorBrush = new SolidColorBrush(userControl.SelectingColor);
            }
        }

    }
}
