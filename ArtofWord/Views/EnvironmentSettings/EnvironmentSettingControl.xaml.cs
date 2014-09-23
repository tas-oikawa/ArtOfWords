using CommonControls;
using CommonControls.Util;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArtOfWords.Models
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class EnvironmentSettingView : UserControl
    {
        public EnvironmentSettingView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
        }

        private void FontButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dlg = new emanual.Wpf.Controls.FontDialogEx();
            CommonLightBox lightBox = new CommonLightBox();
            lightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveCancel;
            lightBox.BindUIElement(dlg);
            lightBox.Owner = Application.Current.MainWindow;

            dlg.DlgFontFamily = Properties.Settings.Default.TextBoxFontFamily;
            //dlg.DlgFontWeight = FontWeights.Bold;
            dlg.DlgFontSize = Properties.Settings.Default.TextBoxFontSize;
            dlg.DlgLanguage = "ja-jp";
            dlg.DlgSampleText = "ここでFontのテストが出来ます";

            if (ShowDialogManager.ShowDialog(lightBox) == true)
            {
                Properties.Settings.Default.TextBoxFontSize = dlg.DlgFontSize;
                Properties.Settings.Default.TextBoxFontFamily = dlg.DlgFontFamily;
                Properties.Settings.Default.Save();

                EventAggregator.OnFontSettingChanged(this, 0);
            }
        }

        private void TextStyleButton_Click_1(object sender, RoutedEventArgs e)
        {
            var userControl = new TextBoxColorPicker();
            CommonLightBox lightBox = new CommonLightBox();
            lightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveCancel;
            lightBox.BindUIElement(userControl);
            lightBox.Owner = Application.Current.MainWindow;

            userControl.SelectingFontFamily = Properties.Settings.Default.TextBoxFontFamily;
            //dlg.DlgFontWeight = FontWeights.Bold;
            userControl.SelectingFontSize = Properties.Settings.Default.TextBoxFontSize;
            userControl.SelectingFontColor = Properties.Settings.Default.TextBoxFontColor;
            userControl.SelectingBackgroundColor = Properties.Settings.Default.TextBoxBackgroundColor;

            if (ShowDialogManager.ShowDialog(lightBox) == true)
            {
                Properties.Settings.Default.TextBoxFontColor = userControl.SelectingFontColor;
                Properties.Settings.Default.TextBoxBackgroundColor = userControl.SelectingBackgroundColor;
                Properties.Settings.Default.Save();

                EventAggregator.OnFontSettingChanged(this, 0);
            }
        }
    }
}
