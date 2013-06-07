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
using System.Windows.Shapes;

namespace emanual.Wpf.Controls
{
	public partial class WpfColorDialog : Window
	{
		private Color FSelectedColor = Colors.Red;
		private Window FOwnerWindow = null;
		private int FLeftOffset = 50;
		private int FTopOffset  = 50;

		// プロパティ
		public Color SelectedColor
		{
			get { return FSelectedColor; }
			set { FSelectedColor = value; }
		}

		// ダイアログの表示位置の基準とするウインドウ
		public Window OwnerWindow
		{
			get { return FOwnerWindow; }
			set { FOwnerWindow = value; }
		}

		public int LeftOffset
		{
			get { return FLeftOffset; }
			set { FLeftOffset = value; }
		}

		public int TopOffset
		{
			get { return FTopOffset; }
			set { FTopOffset = value; }
		}
		//---------------------------------------------------------------------------------------------
		public WpfColorDialog()
		{
			InitializeComponent();
		}

		private void control_Loaded(object sender, RoutedEventArgs e)
		{
			// ダイアログの表示位置をオーナーウインドウのちょっと右下にする
			if (FOwnerWindow != null)
			{
				this.Left = FOwnerWindow.Left + FLeftOffset;
				this.Top = FOwnerWindow.Top + FTopOffset;
			}

			colorPicker.SelectedColor = FSelectedColor;
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			FSelectedColor = colorPicker.SelectedColor;

			// カラー値をあらわす文字列をクリップボードにコピーする
			Clipboard.SetText(FSelectedColor.ToString());

			this.DialogResult = true;
			this.Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
	} // end of WpfColorDialog class
} // end of namespace
