/* WPF 対応フォント選択ダイアログボックス

  このクラスは Window クラスから派生するので、基本的な使い方は Window クラスと同じです。
  そのため、FontFamily プロパティなどのプロパティ名が重複します。そこで、プロパティ名の先頭に Dlg を付加しました。
	たとえば、DlgFontFamily といった具合です。

	【著作権】
		本クラスはフリーウエアです。コードの改変・流用は無制限に許可します。ただし、このクラスを使ったことによる
		すべての不都合に対して著作権者は免責されるものとします。

		2012/09/11
		佐藤 正
		e-mail   : pmansato@kanazawa-net.ne.jp
	  HomePage : http://www.kanazawa-net.ne.jp/~pmansato/

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new emanual.Wpf.Controls.FontDialogEx();
			dlg.Left = this.Left + 50;
			dlg.Top = this.Top + 50;
			dlg.DlgFontFamily = new FontFamily("メイリオ");
			//dlg.DlgFontWeight = FontWeights.Bold;
			dlg.DlgFontSize = 15;
			dlg.DlgLanguage = "ja-jp";
			dlg.DlgSampleText = "テスト Test";

			if (ShowDialogManager.ShowDialog(dlg) == true)
			{
				textBox1.FontFamily = dlg.DlgFontFamily;
				textBox1.FontSize = dlg.DlgFontSize;
				textBox1.FontWeight = dlg.DlgFontWeight;
				textBox1.FontStyle = dlg.DlgFontStyle;
			}
		}
*/
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Markup; // XmlLanguage
using System.Windows.Input;
using System.Diagnostics;
using ArtOfWords.ViewModels.EnvironmentSettings; // Debug

namespace emanual.Wpf.Controls
{
    public partial class FontDialogEx : UserControl
    {
        // メンバ変数と初期値
        private FontFamily FFontFamily = new FontFamily("Meiryo UI");
        private double FFontSize = 13.0;
        private FontStyle FFontStyle = FontStyles.Normal;
        private FontWeight FFontWeight = FontWeights.Regular;
        private string FLanguage = "ja-JP";
        private XmlLanguage FXmlLanguage;
        private string FSampleText = "サンプル文字列\r\nSample Text";

        private double[] FFontSizeArray = new double[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 20, 22, 24, 26, 28, 30, 32, 36, 48, 64, 72 };

        #region property
        // プロパティ（Window クラスのプロパティ名と重複するので、Dlg ～ を付加した）
        public FontFamily DlgFontFamily { get { return FFontFamily; } set { FFontFamily = value; } }
        public double DlgFontSize { get { return FFontSize; } set { FFontSize = value; } }
        public FontStyle DlgFontStyle { get { return FFontStyle; } set { FFontStyle = value; } }
        public FontWeight DlgFontWeight { get { return FFontWeight; } set { FFontWeight = value; } }
        public string DlgSampleText { get { return FSampleText; } set { FSampleText = value; } }
        public string DlgLanguage { get { return FLanguage; } set { FLanguage = value; } }
        #endregion
        //-----------------------------------------------------------------------------------------------
        public FontDialogEx()
        {
            InitializeComponent();
        }


        //---------------------------------------------------------------------------------------------
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

        //---------------------------------------------------------------------------------------------
        private void lstFamilyName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFamilyName.Items.Count < 1)
                return;

            ListedFontViewModel item = lstFamilyName.SelectedItem as ListedFontViewModel;

            if (item != null)
            {
                txtFamilyName.Text = item.FontName as string;
                FFontFamily = item.FontFamily as FontFamily;

                this.UpdateSampleText();
            }
        }

        //---------------------------------------------------------------------------------------------
        private void lstTypeface_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        //---------------------------------------------------------------------------------------------
        private void lstFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstFontSize.Items.Count < 1)
                return;

            FFontSize = (double)lstFontSize.SelectedItem;
            txtFontSize.Text = Convert.ToString(FFontSize);
            this.UpdateSampleText();
        }

        //---------------------------------------------------------------------------------------------
        private void cmbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLanguage.Items.Count < 1)
                return;

            ComboBoxItem item = cmbLanguage.SelectedItem as ComboBoxItem;
            FXmlLanguage = item.Tag as XmlLanguage;

            if (FXmlLanguage == null)
                FLanguage = null;
            else
                FLanguage = FXmlLanguage.IetfLanguageTag;

            this.UpdateFamilyName();
        }

        //---------------------------------------------------------------------------------------------
        private void chkBold_Checked(object sender, RoutedEventArgs e)
        {
            txtSample.FontWeight = FontWeights.Bold;
            FFontWeight = FontWeights.Bold;

            this.UpdateSampleText();
        }

        //---------------------------------------------------------------------------------------------
        private void chkBold_Unchecked(object sender, RoutedEventArgs e)
        {
            txtSample.FontWeight = FontWeights.Regular;
            FFontWeight = FontWeights.Regular;

            this.UpdateSampleText();
        }

        //---------------------------------------------------------------------------------------------
        // cmbLanguage の選択に伴って、lstFamilyName を更新する
        private void UpdateFamilyName()
        {
            var list = new List<ListedFontViewModel>();

            int fontIndex = 0;
            // すべての言語のとき
            if (DlgLanguage == null)
            {
                foreach (FontFamily family in Fonts.SystemFontFamilies)
                {
                    LanguageSpecificStringDictionary dic1 = family.FamilyNames;

                    foreach (XmlLanguage lang in dic1.Keys)
                    {
                        ListedFontViewModel item1 = new ListedFontViewModel();

                        string s = dic1[lang] as string;

                        if ((s != null) && (s.Length > 0))
                        {
                            item1.FontName = s;
                            item1.FontFamily = family;
                            item1.SampleString = "Getting Comfortable with Aow";

                            list.Add(item1);
                        }
                    }
                }
            }
            else // 特定の言語のとき
            {
                foreach (FontFamily family in Fonts.SystemFontFamilies)
                {
                    LanguageSpecificStringDictionary dic2 = family.FamilyNames;
                    ListedFontViewModel item2 = new ListedFontViewModel();

                    string s = "";

                    if (dic2.ContainsKey(FXmlLanguage))
                    {
                        s = dic2[FXmlLanguage] as string;

                        if ((s != null) && (s.Length > 0))
                        {
                            item2.FontName = s;
                            item2.FontFamily = family;
                            item2.SampleString = "Kappaと発音してください。";

                            list.Add(item2);
                        }
                    }
                }
            }

            list.Sort(SortComparison);

            fontIndex = Math.Max(list.FindIndex(t => (t.FontFamily as FontFamily).Equals(FFontFamily)), 0);
            
            lstFamilyName.ItemsSource = list;

            lstFamilyName.SelectedIndex = fontIndex;
            txtFamilyName.Text = ((string)list[fontIndex].FontName).ToString();

            lstFamilyName.ScrollIntoView(lstFamilyName.SelectedItem);
        }

        //---------------------------------------------------------------------------------------------
        // 昇順ソートのためのコールバック関数
        private int SortComparison(ListedFontViewModel item1, ListedFontViewModel item2)
        {
            string s1 = item1.FontName as string;
            string s2 = item2.FontName as string;

            return s1.CompareTo(s2);
        }

        //---------------------------------------------------------------------------------------------
        // txtSampleText のフォントを変更する
        private void UpdateSampleText()
        {
            txtSample.FontFamily = new FontFamily(txtFamilyName.Text);
            txtSample.FontSize = Convert.ToDouble(txtFontSize.Text);
        }

        //---------------------------------------------------------------------------------------------
        // cmbLanguage の項目データを設定する
        // このメソッドは、Window_Loadede で一度だけ呼び出される
        private void SetLanguageList()
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = "日本語（ja-jp）";
            XmlLanguage language = XmlLanguage.GetLanguage("ja-jp");
            item.Tag = language;
            cmbLanguage.Items.Add(item);

            item = new ComboBoxItem();
            item.Content = "米国英語（en-us）";
            language = XmlLanguage.GetLanguage("en-us");
            item.Tag = language;
            cmbLanguage.Items.Add(item);

            // （Windows の標準フォントの中にないので削除した）
            //item = new ComboBoxItem();
            //item.Content = "フランス語（fr-FR）";
            //DlgLanguage = System.Windows.Markup.XmlLanguage.GetLanguage("fr-FR");
            //item.Tag = DlgLanguage;
            //cmbLanguage.Items.Add(item);

            //item = new ComboBoxItem();
            //item.Content = "ドイツ語（de-DE）";
            //DlgLanguage = System.Windows.Markup.XmlLanguage.GetLanguage("de-DE");
            //item.Tag = DlgLanguage;
            //cmbLanguage.Items.Add(item);

            item = new ComboBoxItem();
            item.Content = "中国語（zh-cn）";
            language = XmlLanguage.GetLanguage("zh-cn");
            item.Tag = language;
            cmbLanguage.Items.Add(item);

            item = new ComboBoxItem();
            item.Content = "韓国語（ko-kr）";
            language = XmlLanguage.GetLanguage("ko-kr");
            item.Tag = language;
            cmbLanguage.Items.Add(item);

            item = new ComboBoxItem();
            item.Content = "すべての言語";
            item.Tag = null;
            cmbLanguage.Items.Add(item);

            string s = FLanguage.ToLower();

            // 現在の FLanguage に一致する項目を選択状態にする
            if (s == "ja-jp")
                cmbLanguage.SelectedIndex = 0;
            else if (s == "en-us")
                cmbLanguage.SelectedIndex = 1;
            else if (s == "zh-cn")
                cmbLanguage.SelectedIndex = 2;
            else if (s == "zko-kr")
                cmbLanguage.SelectedIndex = 3;
            else
                cmbLanguage.SelectedIndex = 4;
        }

        //---------------------------------------------------------------------------------------------
        // フォントのサイズに基づいて lstFontSize の項目を選択する
        // このメソッドは、Window_Loadede で一度だけ呼び出される
        private void SetFontSizeList()
        {
            int index = 0;
            lstFontSize.Items.Clear();

            for (int i = 0; i < FFontSizeArray.Length; i++)
            {
                lstFontSize.Items.Add(FFontSizeArray[i]);
            }

            // 現在のサイズを選択状態にする
            for (int i = 0; i < FFontSizeArray.Length; ++i)
            {
                if (FFontSizeArray[i] == FFontSize)
                {
                    index = i;
                    break;
                }
            }

            // FFontSize に一致する項目を選択状態にする
            lstFontSize.SelectedIndex = index;
            lstFontSize.ScrollIntoView(lstFontSize.SelectedItem);
        }

        //---------------------------------------------------------------------------------------------
        // DlgLanguage プロパティの設定をチェックする
        private bool CheckLanguage(string s)
        {
            s = s.ToLower();

            // null または String.Empty のとき、すべての言語とみなす
            if ((s == "ja-jp") || (s == "en-us") || (s == "zh-cn") || (s == "ko-kr"))
                return true;
            else
                return false;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            FXmlLanguage = XmlLanguage.GetLanguage(FLanguage);

            this.SetFontSizeList();
            this.SetLanguageList();

            if (this.DlgFontWeight == FontWeights.Bold)
                chkBold.IsChecked = true;

            this.DlgSampleText = FSampleText;
            txtSample.Text = FSampleText;
        }

    } // end of FontDialogEx class
} // end of namespace
