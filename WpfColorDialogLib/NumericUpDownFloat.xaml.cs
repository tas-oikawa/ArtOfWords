using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Diagnostics;

namespace emanual.Wpf.Controls
{
	public partial class NumericUpDownFloat : UserControl
	{
		private bool FIsButtonClick = false;
		public static readonly DependencyProperty ValueProperty;
		public static readonly DependencyProperty IncrementProperty;
		public static readonly DependencyProperty MaximumProperty;
		public static readonly DependencyProperty MinimumProperty;
		public static readonly DependencyProperty TextAlignmentProperty;
		public static readonly DependencyProperty ButtonClickProperty;

		public static readonly RoutedEvent ValueChangedEvent;

		#region property
		// プロパティ
		public float Increment // クリック 1 回あたりの増減量
		{
			get { return (float)this.GetValue(IncrementProperty); }
			set { this.SetValue(IncrementProperty, value); }
		}

		public float Maximum // 最大値
		{
			get { return (float)this.GetValue(MaximumProperty); }
			set { this.SetValue(MaximumProperty, value); }
		}

		public float Minimum // 最小値
		{
			get { return (float)this.GetValue(MinimumProperty); }
			set { this.SetValue(MinimumProperty, value); }
		}

		public bool ButtonClick
		{
			get { return (bool)this.GetValue(ButtonClickProperty); }
			set { this.SetValue(ButtonClickProperty, value); }
		}

		public TextAlignment TextAlignment // 数値の表示位置
		{
			get { return (TextAlignment)this.GetValue(TextAlignmentProperty); }
			set { this.SetValue(TextAlignmentProperty, value); }
		}

		public float Value // 現在値
		{
			get { return (float)this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		// Value プロパティが変化したときに発生するイベントを設定・取得する
		public event RoutedPropertyChangedEventHandler<float> ValueChanged
		{
			add { this.AddHandler(ValueChangedEvent, value); }
			remove { this.RemoveHandler(ValueChangedEvent, value); }
		}
		#endregion
		//---------------------------------------------------------------------------------------------
		static NumericUpDownFloat()
		{
			IncrementProperty = DependencyProperty.Register("Increment", typeof(float), typeof(NumericUpDownFloat), new FrameworkPropertyMetadata(0.01f));
			MaximumProperty = DependencyProperty.Register("Maximum", typeof(float), typeof(NumericUpDownFloat), new FrameworkPropertyMetadata(1.0f));
			MinimumProperty = DependencyProperty.Register("Minimum", typeof(float), typeof(NumericUpDownFloat), new FrameworkPropertyMetadata(0.0f));
			TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(NumericUpDownFloat), new FrameworkPropertyMetadata(TextAlignment.Center));
			ButtonClickProperty = DependencyProperty.Register("ButtonClick", typeof(bool), typeof(NumericUpDownFloat), new FrameworkPropertyMetadata(false));
			ValueProperty = DependencyProperty.Register("Value", typeof(float), typeof(NumericUpDownFloat),
					new FrameworkPropertyMetadata(0.0f, new PropertyChangedCallback(OnValueChanged),
					new CoerceValueCallback(CoerceValueCallback)));

			ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
				RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<float>), typeof(NumericUpDownFloat));
		}

		//---------------------------------------------------------------------------------------------
		// コンストラクタ
		public NumericUpDownFloat()
		{
			InitializeComponent();
		}

		//---------------------------------------------------------------------------------------------
		private void control_Loaded(object sender, RoutedEventArgs e)
		{
			PART_TextBox.TextAlignment = this.TextAlignment;
		}

		//---------------------------------------------------------------------------------------------
		// Value プロパティを検証するコールバックメソッド
		private static object CoerceValueCallback(DependencyObject element, object value)
		{
			float newValue = (float)value;
			NumericUpDownFloat control = (NumericUpDownFloat)element;

			// Maximum と Minimum との間にする
			newValue = Math.Min(control.Maximum, Math.Max(control.Minimum, newValue));

			return newValue;
		}

		//---------------------------------------------------------------------------------------------
		private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			NumericUpDownFloat control = (NumericUpDownFloat)obj;
			var e = new RoutedPropertyChangedEventArgs<float>((float)args.OldValue, (float)args.NewValue, ValueChangedEvent);
			control.OnValueChanged(e);
		}

		//---------------------------------------------------------------------------------------------
		protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<float> args)
		{
			if (FIsButtonClick)
			{
				this.ButtonClick = true;
				FIsButtonClick = false;
			}
			else
				this.ButtonClick = false;

			this.RaiseEvent(args);
		}

		//---------------------------------------------------------------------------------------------
		private void PART_UpButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Value < this.Maximum)
			{
				this.Value += this.Increment;
				this.ButtonClick = true;
				FIsButtonClick = true;
			}
		}

		//---------------------------------------------------------------------------------------------
		private void PART_DownButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Value > this.Minimum)
			{
				this.Value -= this.Increment;
				this.ButtonClick = true;
				FIsButtonClick = true;
			}
		}

		//---------------------------------------------------------------------------------------------
	} // end of NumericUpDownFloat class

	//**********************************************************************************************
	// Value プロパティを所定の有効小数点以下の数値になるように書式化する
	// このコントロールでは、0 ～ 1 の範囲の数値に対して、小数点以下の有効桁数は 2 とした
	internal class ValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string result = "";
			float val = (float)value;

			result = String.Format("{0:0.00}", val);

			return result;
		}

		//-------------------------------------------------------------------------------------
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	} // end of ValueConverter class

} // end of namespace
