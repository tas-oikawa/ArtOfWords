/*  Hue 値による色調選択カラーバー

		Hue の変更にともなってインジケータの位置を変更する。

		【注意事項】
		H=0, S=1, V=1 と H=360, S=1, V=1 とはともに Colors.Red となるので、H の最大値は 359 とした。
*/
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.Diagnostics;

namespace emanual.Wpf.Controls
{
	public class HueGrid : Grid
	{
		private bool FIsMouseDown;

		public static readonly DependencyProperty HueProperty;
		public static readonly DependencyProperty BaseColorProperty;
		public static readonly DependencyProperty IndicatorOffsetProperty;
		public static readonly RoutedEvent HueChangedEvent;

		#region property
		// プロパティ
		// Hue 値（0 ～ 359）
		public int Hue
		{
			get { return (int)GetValue(HueProperty); }
			set { SetValue(HueProperty, value); this.UpdateIndicatorPosition(value); }
		}

		// 現在の Hue と Saturation = 1.0 Brightness = 1.0 のときの基準色
		public Color BaseColor
		{
			get { return (Color)GetValue(BaseColorProperty); }
			set { SetValue(BaseColorProperty, value); }
		}

		// インジケータの位置
		public double IndicatorOffset
		{
			get { return (double)GetValue(IndicatorOffsetProperty); }
			set { SetValue(IndicatorOffsetProperty, value); }
		}

		public event RoutedPropertyChangedEventHandler<int> HueChanged
		{
			add { this.AddHandler(HueChangedEvent, value); }
			remove { this.RemoveHandler(HueChangedEvent, value); }
		}
		#endregion
		//---------------------------------------------------------------------------------------------
		static HueGrid()
		{
			BaseColorProperty = DependencyProperty.Register("BaseColor", typeof(Color), typeof(HueGrid),
				new FrameworkPropertyMetadata(Colors.Red));
			IndicatorOffsetProperty = DependencyProperty.Register("IndicatorOffset", typeof(double), typeof(HueGrid),
				 new FrameworkPropertyMetadata(0.0));
			HueProperty = DependencyProperty.Register("Hue", typeof(int), typeof(HueGrid),
				 new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnHueChanged)));

			HueChangedEvent = EventManager.RegisterRoutedEvent("HueChanged",
				RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(HueGrid));
		}

		//---------------------------------------------------------------------------------------------
		public HueGrid()
		{
		}

		//---------------------------------------------------------------------------------------------
		private static void OnHueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			HueGrid control = (HueGrid)obj;
			var e = new RoutedPropertyChangedEventArgs<int>((int)args.OldValue, (int)args.NewValue, HueChangedEvent);
			control.OnHueChanged(e);
		}

		//---------------------------------------------------------------------------------------------
		protected virtual void OnHueChanged(RoutedPropertyChangedEventArgs<int> args)
		{
			this.RaiseEvent(args);
		}

		//---------------------------------------------------------------------------------------------
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{
			FIsMouseDown = true;
			this.CaptureMouse();
			this.UpdateIndicatorPosition();
		}

		//---------------------------------------------------------------------------------------------
		protected override void OnPreviewMouseMove(MouseEventArgs e)
		{
			if (FIsMouseDown)
			{
				this.UpdateIndicatorPosition();
			}
		}

		//---------------------------------------------------------------------------------------------
		protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
		{
			FIsMouseDown = false;
			this.ReleaseMouseCapture();
		}

		//---------------------------------------------------------------------------------------------
		// 色調選択バーをクリックまたはマウスドラッグしたときに、その位置の Hue 値を設定し、インジケータを移動する
		private void UpdateIndicatorPosition()
		{
			Point p = Mouse.GetPosition(this);
			double offset = p.Y;

			offset = Math.Max(0, Math.Min(this.ActualHeight, p.Y));

			this.Hue = 360 - Convert.ToInt32(360 * offset / this.ActualHeight);
			this.Hue = ColorUtility.ClampColorValue(this.Hue, 0, 359);
			this.IndicatorOffset = offset;

			// BaseColor プロパティを設定する
			this.BaseColor = ColorUtility.HsvToColor(this.Hue, 1.0, 1.0);
		}

		//---------------------------------------------------------------------------------------------
		// Hue 値に基づいてインジケータの位置を調整し、BaseColor プロパティを設定する
		private void UpdateIndicatorPosition(int hue)
		{
			this.IndicatorOffset = (360 - hue) * this.ActualHeight / 360;
			this.BaseColor = ColorUtility.HsvToColor(hue, 1.0, 1.0);
		}

		//---------------------------------------------------------------------------------------------
	} // end of HueGrid class
} // end of namespace
