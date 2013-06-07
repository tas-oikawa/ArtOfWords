using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;
using System.Diagnostics;

namespace emanual.Wpf.Controls
{
	public class ColorUtility
	{
		// int 型値を指定の範囲内に宣言する
		public static int ClampColorValue(int colorValue, int min, int max)
		{
			int result = colorValue;

			if (colorValue < min)
				result = min;

			if (colorValue > max)
				result = max;

			return result;
		}

		//---------------------------------------------------------------------------------------------
		// double 型値を指定の範囲内に宣言する
		public static double ClampColorValue(double colorValue, double min, double max)
		{
			double result = colorValue;

			if (colorValue < min)
				result = min;

			if (colorValue > max)
				result = max;

			return result;
		}

		//---------------------------------------------------------------------------------------------
		// カラー値の文字列表現を SelectedColor 型に変換する
		// s : カラー値の文字列表現（例： #RRGGBB、"#RGB"、"#AARRGGBB"、"#ARGB"）
		public static Color ConvertColorFromString(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return Colors.White;
			}

			if (s[0] != '#')
				s = "#" + s;

			try
			{
				return (Color)ColorConverter.ConvertFromString(s);
			}
			catch
			{
				return Colors.White;
			}
		}

		//---------------------------------------------------------------------------------------------
		// HSV 値を SelectedColor 型に変換する
		// h : Hue (0 ～ 360) 
		// s : Saturation (0 ～ 1)
		// v : Brightness (0 ～ 1)
		public static Color HsvToColor(double h, double s, double v)
		{
			double r, g, b;

			HsvToRgb(h, s, v, out r, out g, out b);

			return Color.FromRgb((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
		}

		//---------------------------------------------------------------------------------------------
		// SelectedColor 型を HSV 値に変換する
		public static void ColorToHsv(Color color, out double h, out double s, out double v)
		{
			RgbToHsv(color.R / 255.0, color.G / 255.0, color.B / 255.0, out h, out s, out v);
		}

		//---------------------------------------------------------------------------------------------
		// RGB 値を HSV 値に変換する
		// r : Red (0 ～ 1) 
		// g : Green (0 ～ 1)
		// b : Blue (0 ～ 1)
		public static void RgbToHsv(double r, double g, double b, out double h, out double s, out double v)
		{
			var max = Math.Max(r, Math.Max(g, b));

			if (max == 0.0) // max = 0 は Black で、h、s、v はすべて 0
			{
				h = s = v = 0.0;
				return;
			}

			var min = Math.Min(r, Math.Min(g, b));
			double delta = max - min;

			if (max == min)
			{
				h = 0;
			}
			else if (max == r)
			{
				h = (60 * (g - b) / delta) % 360;
			}
			else if (max == g)
			{
				h = 60 * (b - r) / delta + 120;
			}
			else
			{
				h = 60 * (r - g) / delta + 240;
			}

			if (h < 0.0)
				h += 360;

			h = Math.Round(h, MidpointRounding.AwayFromZero);
			s = delta / max;

			v = max;
		}

		//---------------------------------------------------------------------------------------------
		// HSV 値を RGB 値に変換する
		// h : Hue (0 ～ 360) 
		// s : Saturation (0 ～ 1)
		// v : Brightness (0 ～ 1)
		public static void HsvToRgb(double h, double s, double v, out double r, out double g, out double b)
		{
			if (s == 0.0)
			{
				r = v;
				g = v;
				b = v;

				return;
			}

			h = h % 360;
			int hi = (int)(h / 60) % 6;
			var f = h / 60 - (int)(h / 60);
			var p = v * (1 - s);
			var q = v * (1 - f * s);
			var t = v * (1 - (1 - f) * s);

			switch (hi)
			{
				case 0: r = v; g = t; b = p; break;
				case 1: r = q; g = v; b = p; break;
				case 2: r = p; g = v; b = t; break;
				case 3: r = p; g = q; b = v; break;
				case 4: r = t; g = p; b = v; break;
				default: r = v; g = p; b = q; break;
			}
		}

		//---------------------------------------------------------------------------------------------
		// System.Windows.Media.SelectedColor 型から HSL 値に変換する
		public static void RgbToHsl(System.Windows.Media.Color color, ref double hue, ref double saturation, ref double lightness)
		{
			double r, g, b, h, s, l;

			r = color.R / 255.0;
			g = color.G / 255.0;
			b = color.B / 255.0;

			double maxColor = Math.Max(r, Math.Max(g, b));
			double minColor = Math.Min(r, Math.Min(g, b));

			if (r == g && r == b)
			{
				h = 0.0;
				s = 0.0;
				l = r;
			}
			else
			{
				l = (minColor + maxColor) / 2;
				if (l < 0.5)
					s = (maxColor - minColor) / (maxColor + minColor);
				else
					s = (maxColor - minColor) / (2.0 - maxColor - minColor);

				if (r == maxColor)
					h = (g - b) / (maxColor - minColor);
				else if (g == maxColor)
					h = 2.0 + (b - r) / (maxColor - minColor);
				else
					h = 4.0 + (r - g) / (maxColor - minColor);

				h /= 6;

				if (h < 0)
					++h;
			}

			// 0 ～ 1 の範囲内に制限する
			if (h < 0) h = 0;
			if (h > 1) h = 1;
			if (s < 0) s = 0;
			if (s > 1) s = 1;
			if (l < 0) l = 0;
			if (l > 1) l = 1;

			hue = h * 360;
			saturation = s;
			lightness = l;
		}

		//---------------------------------------------------------------------------------------------
		// HSL 値から System.Windows.Media.SelectedColor 型に変換する
		// hue        : 0 ～ 360
		// saturation : 0 ～ 1
		// lightness  : 0 ～ 1
		public static System.Windows.Media.Color HslToRgb(double hue, double saturation, double lightness)
		{
			double r, g, b, h, s, l, s1, s2, r1, g1, b1;

			h = hue / 360.0;
			s = saturation;
			l = lightness;

			if (s == 0)
			{
				r = g = b = l;
			}
			else
			{
				if (l < 0.5)
				{
					s2 = l * (1 + s);
				}
				else
				{
					s2 = (l + s) - (l * s);
				}

				s1 = 2 * l - s2;
				r1 = h + 1.0 / 3.0;

				if (r1 > 1)
				{
					--r1;
				}

				g1 = h;
				b1 = h - 1.0 / 3.0;

				if (b1 < 0)
					++b1;

				// R
				if (r1 < 1.0 / 6.0)
					r = s1 + (s2 - s1) * 6.0 * r1;
				else if (r1 < 0.5)
					r = s2;
				else if (r1 < 2.0 / 3.0)
					r = s1 + (s2 - s1) * ((2.0 / 3.0) - r1) * 6.0;
				else
					r = s1;

				// G
				if (g1 < 1.0 / 6.0)
					g = s1 + (s2 - s1) * 6.0 * g1;
				else if (g1 < 0.5)
					g = s2;
				else if (g1 < 2.0 / 3.0)
					g = s1 + (s2 - s1) * ((2.0 / 3.0) - g1) * 6.0;
				else g = s1;

				// B
				if (b1 < 1.0 / 6.0)
					b = s1 + (s2 - s1) * 6.0 * b1;
				else if (b1 < 0.5)
					b = s2;
				else if (b1 < 2.0 / 3.0)
					b = s1 + (s2 - s1) * ((2.0 / 3.0) - b1) * 6.0;
				else
					b = s1;
			}

			// 0 ～ 1 の範囲内に制限する
			if (r < 0) r = 0;
			if (r > 1) r = 1;
			if (g < 0) g = 0;
			if (g > 1) g = 1;
			if (b < 0) b = 0;
			if (b > 1) b = 1;

			return Color.FromRgb(Convert.ToByte(r * 255), Convert.ToByte(g * 255), Convert.ToByte(b * 255));
		}

		//---------------------------------------------------------------------------------------------
		// 背景色に対して視認しやすい文字色として、白色または黒色を返す
		// backgroundColor : 背景色
		public static Color GetInvertTextColor(Color backgroundColor)
		{
			Color color = Colors.Black;

			// 背景色をグレースケールに変換し、明るさの基準とする
		  double grayScale = 0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B;

			// 128 より明るいとき
		  if (grayScale > 128)
				color = Colors.Black;
		  else
				color = Colors.White;

			return color;
		}

	} // end of BrushEditorUtility class
} // end of namespace
