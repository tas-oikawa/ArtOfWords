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
using WritersBattleField;
using ArtOfWords.ViewModels;
using WritersBattleField.View;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using CommonControls;
using System.IO;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using CommonControls.Util;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System.Threading;
using ModernizedAlice.ArtOfWords.BizCommon.Util;
using ArtOfWords.Views.Satelite;

namespace ArtOfWords.Views.Main
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindowView : Window
    {
        #region ViewModels
        private MainWindowViewModel _model;
        #endregion


        public MainWindowView()
        {
            InitializeComponent();

            this.Left = Properties.Settings.Default.WindowPos.Left;
            this.Top = Properties.Settings.Default.WindowPos.Top;
            this.Width = Properties.Settings.Default.WindowPos.Width;
            this.Height = Properties.Settings.Default.WindowPos.Height;
            if (Properties.Settings.Default.IsMaximized)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }

            SetDefaultFont();
        }

        private void SetDefaultFont()
        {
            var font = App.Current.FindResource("CommonFontFamily") as FontFamily;
            var selectiveFont = Properties.Settings.Default["TextBoxFontFamily"] as FontFamily;

            if (DoesFontExist(selectiveFont))
            {
                font = selectiveFont;
            }
            else
            {
                font = new FontFamily("MS Gothic");
                Properties.Settings.Default["TextBoxFontFamily"] = font;
            }
        }

        public bool DoesFontExist(FontFamily fontFamily)
        {
            if (fontFamily == null)
            {
                return false;
            }

            foreach (var f in System.Windows.Media.Fonts.SystemFontFamilies)
            {
                if (f.Source.Equals(fontFamily.Source))
                {
                    return true;
                }
            }
            return false;
        }

        public void InitializeViews()
        {
            _model.InitializeViews();
        }

        public void BindData(MainWindowViewModel model)
        {
            _model = model;

            this.DataContext = model;
        }

        private void SaveState()
        {
            Properties.Settings.Default.WindowPos = new Rect(this.Left, this.Top, this.Width, this.Height);
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                Properties.Settings.Default.IsMaximized = true;
            }
            else
            {
                Properties.Settings.Default.IsMaximized = false;
            }
            Properties.Settings.Default.Save();
        }

        #region View内のコントロールを取得する

        public WritersBattleFieldView GetWritersBattleFieldView()
        {
            return writersBattlefield.Content as WritersBattleFieldView;
        }

        public TimelineControl.Timeline GetTimelineControl()
        {
            return TimelineControll.Content as TimelineControl.Timeline;
        }

        public WebBrowser GetAdsWebBrowser()
        {
            return AdsWebBrowser.Content as WebBrowser;
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // ファイル権限とかチェックする
            FileAuthorizeChecker.CheckAndRepairAuthorize();

            _model = new MainWindowViewModel(this);
            InitializeViews();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // CancelをTrueにするとクローズキャンセル
            e.Cancel = (!_model.TryClose());

            if (e.Cancel != true)
            {
                ClosingScreen closing = new ClosingScreen();
                closing.Owner = this;
                closing.Width = this.ActualWidth;
                closing.Height = this.ActualHeight;
                closing.Show();

                SaveState();

                Thread.Sleep(1000);
                closing.Close();
            }
        }

        // コマンドが行う処理
        private void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            EventAggregator.OnTrySave(this, new TrySaveOccuredEventArgs()
            {
                SaveKind = SaveKind.SaveOverWrite
            });
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.kanazawa-net.ne.jp/~pmansato/");
            }
            catch { }
        }

        private void Hyperlink_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://main.tinyjoker.net/");
            }
            catch { }
        }

        private void WebBrowser_Navigating_1(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri.OriginalString != _model.AdsPage)
            {
                System.Diagnostics.Process.Start(e.Uri.OriginalString);
                e.Cancel = true;
            }
        }

        private void SateliteButton_Click(object sender, RoutedEventArgs e)
        {
            SateliteSelector sateliteSelector = new SateliteSelector();
            CommonLightBox commonLightBox = new CommonLightBox();

            commonLightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.CancelOnly;
            commonLightBox.BindUIElement(sateliteSelector);
            commonLightBox.Owner = Application.Current.MainWindow;

            ShowDialogManager.ShowDialog(commonLightBox);
        }

        private void Hyperlink_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://davidowens.wordpress.com/");
            }
            catch { }
            
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            if (_model == null)
            {
                return;
            }
            _model.OnWindowActivated();
        }

        private void Hyperlink_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://code.google.com/p/gong-wpf-dragdrop/");
            }
            catch { }
        }


        /// <summary>
        /// このソフトについてスイッチクリック
        /// </summary>
        private void AboutSwitch_Click(object sender, RoutedEventArgs e)
        {
            var window = new AboutBoxWindow();
            window.Owner = this;
            window.ShowDialog();
        }
    }
}
