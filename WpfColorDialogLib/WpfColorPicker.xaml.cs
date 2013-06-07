using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace emanual.Wpf.Controls
{
	public partial class WpfColorPicker : UserControl
	{
		private const int CELL_WIDTH = 10;
		private const int CELL_HEIGHT = 10;
		private List<CellColor> SVList;
		private Color FOldSelectedColor = Colors.Red;

		public static readonly DependencyProperty SelectedColorProperty;
		public static readonly RoutedEvent SelectedColorChangedEvent;

		public static readonly DependencyProperty AlphaProperty;
		public static readonly DependencyProperty RedProperty;
		public static readonly DependencyProperty GreenProperty;
		public static readonly DependencyProperty BlueProperty;

		public static readonly DependencyProperty HueProperty;
		public static readonly DependencyProperty SaturationProperty;
		public static readonly DependencyProperty BrightnessProperty;

		#region property
		public Color SelectedColor
		{
			get { return (Color)this.GetValue(SelectedColorProperty); }
			set { this.SetValue(SelectedColorProperty, value); }
		}

		public byte Alpha
		{
			get { return (byte)this.GetValue(AlphaProperty); }
			set { this.SetValue(AlphaProperty, value); }
		}

		public byte Red
		{
			get { return (byte)this.GetValue(RedProperty); }
			set { this.SetValue(RedProperty, value); }
		}

		public byte Green
		{
			get { return (byte)this.GetValue(GreenProperty); }
			set { this.SetValue(GreenProperty, value); }
		}

		public byte Blue
		{
			get { return (byte)this.GetValue(BlueProperty); }
			set { this.SetValue(BlueProperty, value); }
		}

		public int Hue
		{
			get { return (int)this.GetValue(HueProperty); }
			set { this.SetValue(HueProperty, value); }
		}

		public double Saturation
		{
			get { return (double)this.GetValue(SaturationProperty); }
			set { this.SetValue(SaturationProperty, value); }
		}

		public double Brightness
		{
			get { return (double)this.GetValue(BrightnessProperty); }
			set { this.SetValue(BrightnessProperty, value); }
		}

		// SelectedColor プロパティが変化したときに発生するイベントを設定・取得する
		public event RoutedPropertyChangedEventHandler<Color> SelectedColorChanged
		{
			add { this.AddHandler(SelectedColorChangedEvent, value); }
			remove { this.RemoveHandler(SelectedColorChangedEvent, value); }
		}
		#endregion

		//---------------------------------------------------------------------------------------------
		static WpfColorPicker()
		{
			SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(WpfColorPicker),
				new FrameworkPropertyMetadata(Colors.Transparent, new PropertyChangedCallback(OnSelectedColorChanged)));

			SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged",
				RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<Color>), typeof(WpfColorPicker));

			AlphaProperty = DependencyProperty.Register("Alpha", typeof(byte), typeof(WpfColorPicker), new FrameworkPropertyMetadata((byte)255));
			RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(WpfColorPicker), new FrameworkPropertyMetadata((byte)255));
			GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(WpfColorPicker), new FrameworkPropertyMetadata((byte)0));
			BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(WpfColorPicker), new FrameworkPropertyMetadata((byte)0));

			HueProperty = DependencyProperty.Register("Hue", typeof(int), typeof(WpfColorPicker), new FrameworkPropertyMetadata(0));
			SaturationProperty = DependencyProperty.Register("Saturation", typeof(double), typeof(WpfColorPicker), new FrameworkPropertyMetadata(1.0));
			BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(WpfColorPicker), new FrameworkPropertyMetadata(1.0));
		}

		//---------------------------------------------------------------------------------------------
		public WpfColorPicker()
		{
			InitializeComponent();
		}

		//---------------------------------------------------------------------------------------------
		private void userControl_Loaded(object sender, RoutedEventArgs e)
		{
			PART_HueGrid.HueChanged += new RoutedPropertyChangedEventHandler<int>(PART_HueGrid_HueChanged);

			Color c = Color.FromRgb(this.SelectedColor.R, this.SelectedColor.G, this.SelectedColor.B);

			double h, s, v;
			ColorUtility.ColorToHsv(c, out h, out s, out v);
			this.Hue = (int)h;

			// 色調（Hue）選択用カラーバーを設定する
			this.SetHueColorBar();

			// コントロールを初期化する
			this.SetSVList(this.Hue);
			this.SetUpColorCanvas(this.Hue);

			// アルファ値用スライダーコントロール
			PART_AlphaSlider.Value = this.SelectedColor.A;

			// アップダウンコントロールの Value プロパティを設定する
			this.SetUpdownWithSelectedColor(this.SelectedColor, 0);

			// SelectedColor プロパティを外部から変更したとき（たとえば、初期値として与えられたとき）
			if (FOldSelectedColor != this.SelectedColor)
			{
				int index = 0;

				foreach (object item in PART_UniformGrid.Children)
				{
					Rectangle rect = (Rectangle)item;
					CellColor cellColor = SVList[index];

					if (((s >= cellColor.S1) && (s < cellColor.S2)) && ((v <= cellColor.V1) && (v > cellColor.V2)))
					{
						var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
						this.rect_MouseLeftButtonDown(rect, args);

						break;
					}

					++index;
				}
			}

			PART_HueGrid.Hue = this.Hue;
			FOldSelectedColor = this.SelectedColor;
		}

		//---------------------------------------------------------------------------------------------
		private static void OnSelectedColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			WpfColorPicker control = (WpfColorPicker)obj;
			var e = new RoutedPropertyChangedEventArgs<Color>((Color)args.OldValue, (Color)args.NewValue, SelectedColorChangedEvent);
			control.OnSelectedColorChanged(e);
		}

		//---------------------------------------------------------------------------------------------
		protected virtual void OnSelectedColorChanged(RoutedPropertyChangedEventArgs<Color> args)
		{
			this.RaiseEvent(args);
		}

		//---------------------------------------------------------------------------------------------
		// PART_HueGrid クラスにおいて Hue プロパティ変更したとき
		private void PART_HueGrid_HueChanged(object sender, RoutedPropertyChangedEventArgs<int> args)
		{
			if (this.Hue != args.NewValue)
			{
				this.Hue = args.NewValue;
				PART_HueUpDown.Value = this.Hue;
				this.SetColorCanvasWithHue(this.Hue);

				var rect = PART_UniformGrid.Children[16];
				this.rect_MouseLeftButtonDown(rect, null); // キャンバスの右上隅のセルをクリックする
			}
		}

		//---------------------------------------------------------------------------------------------
		private void PART_AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			byte alpha = System.Convert.ToByte(e.NewValue);

			if (alpha != this.Alpha)
			{
				this.Alpha = alpha;
				Color color = this.SelectedColor;
				color.A = this.Alpha;
				this.SelectedColor = color;
				FOldSelectedColor = this.SelectedColor;
			}
		}

		//---------------------------------------------------------------------------------------------
		// R、G、B 用アップダウンコントロールを操作したとき
		private void PART_RgbUpDown_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<int> args)
		{
			NumericUpDown control = sender as NumericUpDown;

			if (control == null)
				return;

			bool isHandled = false;
			byte newValue = (byte)args.NewValue;
			Color color = this.SelectedColor;

			switch (control.Name)
			{
				case "PART_RedUpDown":
					if (color.R != newValue)
					{
						color.R = newValue;
						this.Red = newValue;
						isHandled = true;
					}
					break;
				case "PART_GreenUpDown":
					if (color.G != newValue)
					{
						color.G = newValue;
						this.Green = newValue;
						isHandled = true;
					}
					break;
				case "PART_BlueUpDown":
					if (color.B != newValue)
					{
						color.B = newValue;
						this.Blue = newValue;
						isHandled = true;
					}
					break;
			}

			if (isHandled)
			{
				this.SelectedColor = color;
				FOldSelectedColor = this.SelectedColor;

				Color c = Color.FromRgb(this.SelectedColor.R, this.SelectedColor.G, this.SelectedColor.B);

				double h, s, v;
				ColorUtility.ColorToHsv(c, out h, out s, out v);

				this.Hue = System.Convert.ToInt32(h);
				this.Saturation = System.Convert.ToSingle(s);
				this.Brightness = System.Convert.ToSingle(v);
			}
		}

		//---------------------------------------------------------------------------------------------
		private void PART_Hexadecimal_KeyDown(object sender, KeyEventArgs e)
		{
			// 入力後、[Return] を押したとき
			if (e.Key == Key.Return)
			{
				this.SelectedColor = ColorUtility.ConvertColorFromString(PART_Hexadecimal.Text);
				FOldSelectedColor = this.SelectedColor;

				// アップダウンコントロールの Value プロパティを設定する
				this.SetUpdownWithSelectedColor(this.SelectedColor, 1);

				Color c = Color.FromRgb(this.SelectedColor.R, this.SelectedColor.G, this.SelectedColor.B);

				// アルファ値用スライダーコントロール
				PART_AlphaSlider.Value = this.SelectedColor.A;

				double h, s, v;
				ColorUtility.ColorToHsv(c, out h, out s, out v);

				// Hue 値に基づいてカラーキャンバスを設定する
				PART_HueGrid.Hue = (int)h;
				this.SetColorCanvasWithHue((int)h);

				int index = 0;

				foreach (object item in PART_UniformGrid.Children)
				{
					Rectangle rect = (Rectangle)item;
					CellColor cellColor = SVList[index];

					if (((s >= cellColor.S1) && (s < cellColor.S2)) && ((v <= cellColor.V1) && (v > cellColor.V2)))
					{
						var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left);
						this.rect_MouseLeftButtonDown(rect, args);

						break;
					}

					++index;
				}
			}
		}

		//-----------------------------------------------------------------------------
		// セル内をマウスダウンしたとき
		private void rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Rectangle rect = sender as Rectangle;

			if (rect != null)
			{
				Color color = (Color)rect.Tag;
				//Color c1 = Color.FromRgb(SelectedColor.R, SelectedColor.G, SelectedColor.B);

				this.SelectedColor = Color.FromArgb(this.Alpha, color.R, color.G, color.B);
				FOldSelectedColor = this.SelectedColor;

				// SelectedColor に基づいて各アップダウンコントロールの Value を設定する
				this.SetUpdownWithSelectedColor(this.SelectedColor, 2);

				// 現在のフォーカス矩形を消去する
				this.RemoveFocusingOfRect();

				// マウスダウンした位置のセルを強調表示する
				rect.Width += 2;
				rect.Height += 2;
				rect.Stroke = new SolidColorBrush(this.GetFocusRectColor(this.Hue));
				rect.StrokeThickness = 2;
			}
		}

		//-----------------------------------------------------------------------------
		// 指定の Hue 値に対応するフォーカス矩形の色を取得する
		// このメソッドは、rect_MouseLeftButtonDown イベント内で呼ばれる
		private Color GetFocusRectColor(int hue)
		{
			if ((hue > 40) && (hue < 260))
				return Colors.Red;
			else
				return Colors.Lime;
		}

		//-----------------------------------------------------------------------------
		// マウスダウンした位置にあるセルのフォーカス矩形を無効にする
		// このメソッドは、rect_MouseLeftButtonDown イベント内で呼ばれる
		private void RemoveFocusingOfRect()
		{
			foreach (object item in PART_UniformGrid.Children)
			{
				Rectangle rect = (Rectangle)item;

				if (rect.ActualWidth > CELL_WIDTH)
				{
					rect.Width -= 2;
					rect.Height -= 2;
					rect.StrokeThickness = 0;
					break;
				}
			}
		}

		//---------------------------------------------------------------------------------------------
		// 指定の SelectedColor 型に基づいて、各アップダウンコントロールの Value プロパティを設定する
		// このメソッドは、userControl_Loaded、PART_Hexadecimal_KeyDown、rect_MouseLeftButtonDown イベント内で呼ばれる
		// n はどこから呼ばれたかをチェックするための番号で、デバッグ目的にしか使わない
		private void SetUpdownWithSelectedColor(Color color, int n)
		{
//			Debug.Print(String.Format("SetUpdownWithSelectedColor: {0} {1}", n, color));

			this.Alpha = color.A;
			this.Red = color.R;
			this.Green = color.G;
			this.Blue = color.B;

			Color c = Color.FromRgb(color.R, color.G, color.B);

			double h, s, v;
			ColorUtility.ColorToHsv(c, out h, out s, out v);

			this.Hue = System.Convert.ToInt32(h);
			this.Saturation = System.Convert.ToSingle(s);
			this.Brightness = System.Convert.ToSingle(v);
		}

		//---------------------------------------------------------------------------------------------
		// Hue 値に基づいてカラーキャンバスの各セルの色を設定する
		// このメソッドは、PART_HueGrid_HueChanged イベント内で呼ばれる
		private void SetColorCanvasWithHue(int hue)
		{
			int index = 0;

			foreach (object item in PART_UniformGrid.Children)
			{
				Rectangle rect = (Rectangle)item;

				CellColor cellColor = SVList[index];
				Color newColor = ColorUtility.HsvToColor((double)hue, cellColor.S1, cellColor.V1);

				rect.Fill = new SolidColorBrush(newColor);
				rect.Tag = newColor;

				++index;
			}
		}

		//---------------------------------------------------------------------------------------------
		// 指定の Hue 値に基づいて、キャンバスにコントロールを配置し、初期化する
		// このメソッドは、userControl_Loaded イベント内で 1 回だけ呼ばれる
		private void SetUpColorCanvas(int hue)
		{
			int n = 0;
			PART_UniformGrid.Children.Clear();

			for (double v = 1.0; v >= 0.0; v -= 0.0625)
			{
				for (double s = 0.0; s <= 1.0; s += 0.0625)
				{
					var rect = new Rectangle();
					rect.Width = CELL_WIDTH;
					rect.Height = CELL_HEIGHT;

					CellColor sv = SVList[n++];
					Color color = ColorUtility.HsvToColor((double)hue, sv.S1, sv.V1);
					rect.Fill = new SolidColorBrush(color);
					rect.Tag = color;
					rect.MouseLeftButtonDown += new MouseButtonEventHandler(rect_MouseLeftButtonDown);

					PART_UniformGrid.Children.Add(rect);
				}
			}
		}

		//---------------------------------------------------------------------------------------------
		// 各セルの Saturation および Brightness 値をリストに格納する
		// このメソッドは、userControl_Loaded イベント内で 1 回だけ呼ばれる
		private void SetSVList(int hue)
		{
			int n = 0;
			const double N = 0.0625;
			SVList = new List<CellColor>(289); // = 17 x 17

			for (double v = 1.0; v >= 0.0; v -= N)
			{
				for (double s = 0.0; s <= 1.0; s += N)
				{
					var sv = new CellColor();
					sv.H = hue;
					sv.S1 = s;
					sv.S2 = s + N;
					sv.V1 = v;
					sv.V2 = v - N;
					sv.Color = ColorUtility.HsvToColor(hue, s, v);

					++n;

					SVList.Add(sv);
				}
			}
		}

		//---------------------------------------------------------------------------------------------
		// 色調（Hue）選択用カラーバーを設定する
		// このメソッドは userControl_Loaded イベント内で一度だけ呼ばれる
		private void SetHueColorBar()
		{
			var brush = new LinearGradientBrush();
			brush.StartPoint = new Point(0.5, 1);
			brush.EndPoint = new Point(0.5, 0);
			brush.ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation;

			int segments = 20; // BaseColor のセグメント数
			double offset = (double)1 / segments; // 1 セグメントの Offset 量
			double n = (double)360 / segments;    // 1 セグメントあたりの Hue 変化量

			for (int i = 0; i <= segments; ++i)
			{
				Color color = ColorUtility.HslToRgb(i * n, 1, 0.5);
				brush.GradientStops.Add(new GradientStop(color, i * offset));
			}

			PART_HueGrid.Background = brush;
		}

		//---------------------------------------------------------------------------------------------
		// セルの H, S, V, SelectedColor 値を保持する
		private struct CellColor
		{
			public double H;    // セルの Hue 値
			public double S1;   // 同、Saturation の最小値
			public double S2;   // 同、最大値
			public double V1;   // セルの Brightness 値の最小値
			public double V2;   // 同、最大値
			public Color Color; // セルの色
		}
	}

	//**********************************************************************************************
	// 変換クラス
	//**********************************************************************************************
	// SelectedColor 値 16 進数に書式化する
	internal class HexadecimalValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Color color = (Color)value;

			return color.ToString();
		}

		//-------------------------------------------------------------------------------------
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	} // end of HexadecimalValueConverter class

	//**********************************************************************************************
	// Alpha 値を書式化し、PART_AlphaLider の値を表示する
	internal class AlphaValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int val = System.Convert.ToInt32(value);

			return String.Format("{0,3:d}", val);
		}

		//-------------------------------------------------------------------------------------
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	} // end of AlphaValueConverter class

} // end of namespace
