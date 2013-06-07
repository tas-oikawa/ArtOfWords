using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace emanual.Wpf.Controls
{
	public partial class NumericUpDown : UserControl
	{
		private bool FIsButtonClick = false;

		public static readonly DependencyProperty IncrementProperty;
		public static readonly DependencyProperty MaximumProperty;
		public static readonly DependencyProperty MinimumProperty;
		public static readonly DependencyProperty ValueProperty;
		public static readonly DependencyProperty TextAlignmentProperty;
		public static readonly DependencyProperty ButtonClickProperty;

		public static readonly RoutedEvent ValueChangedEvent;

		#region property
		// プロパティ
		public int Increment // クリック 1 回あたりの増減量
		{
			get { return (int)this.GetValue(IncrementProperty); }
			set { this.SetValue(IncrementProperty, value); }
		}

		public int Maximum
		{
			get { return (int)this.GetValue(MaximumProperty); }
			set { this.SetValue(MaximumProperty, value); }
		}

		public int Minimum
		{
			get { return (int)this.GetValue(MinimumProperty); }
			set { this.SetValue(MinimumProperty, value); }
		}

		public bool ButtonClick // アップダウンボタンをユーザーがクリックしたとき true
		{
			get { return (bool)this.GetValue(ButtonClickProperty); }
			set { this.SetValue(ButtonClickProperty, value); }
		}

		public TextAlignment TextAlignment // 数値の表示位置
		{
			get { return (TextAlignment)this.GetValue(TextAlignmentProperty); }
			set { this.SetValue(TextAlignmentProperty, value); }
		}

		public int Value // 現在値
		{
			get { return (int)this.GetValue(ValueProperty); }
			set { this.SetValue(ValueProperty, value); }
		}

		// Value プロパティが変化したときに発生するイベントを設定・取得する
		public event RoutedPropertyChangedEventHandler<int> ValueChanged
		{
			add { this.AddHandler(ValueChangedEvent, value); }
			remove { this.RemoveHandler(ValueChangedEvent, value); }
		}
		#endregion
		//---------------------------------------------------------------------------------------------
		static NumericUpDown()
		{
			IncrementProperty = DependencyProperty.Register("Increment", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(1));
			MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(255));
			MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(NumericUpDown), new FrameworkPropertyMetadata(0));
			ButtonClickProperty = DependencyProperty.Register("ButtonClick", typeof(bool), typeof(NumericUpDown), new FrameworkPropertyMetadata(false));
			TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(NumericUpDown), new FrameworkPropertyMetadata(TextAlignment.Center));
			ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown),
					new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueChanged),
					new CoerceValueCallback(CoerceValueCallback)));

			ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
				RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumericUpDown));
		}

		//---------------------------------------------------------------------------------------------
		// コンストラクタ
		public NumericUpDown()
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
			int newValue = (int)value;
			NumericUpDown control = (NumericUpDown)element;

			// Maximum と Minimum との間にする
			newValue = Math.Min(control.Maximum, Math.Max(control.Minimum, newValue));

			return newValue;
		}

		//---------------------------------------------------------------------------------------------
		private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			NumericUpDown control = (NumericUpDown)obj;

			RoutedPropertyChangedEventArgs<int> e = new RoutedPropertyChangedEventArgs<int>(
					(int)args.OldValue, (int)args.NewValue, ValueChangedEvent);

			control.OnValueChanged(e);
		}

		//---------------------------------------------------------------------------------------------
		protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<int> args)
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
	} // end of NumericUpDown class
} // end of namespace
