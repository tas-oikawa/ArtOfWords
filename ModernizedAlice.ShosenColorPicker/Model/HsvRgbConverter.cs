using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ModernizedAlice.ShosenColorPicker.Model
{
        /// <summary>
        /// HSV (HSB) カラーを表す
        /// </summary>
        public class HsvColor
        {
            private float _h;
            /// <summary>
            /// 色相 (Hue)
            /// </summary>
            public float H
            {
                set { _h = value; }
                get { return this._h; }
            }

            private float _s;
            /// <summary>
            /// 彩度 (Saturation)
            /// </summary>
            public float S
            {
                set
                {
                    _s = value;
                }
                get { return this._s; }
            }

            private float _v;
            /// <summary>
            /// 明度 (Value, Brightness)
            /// </summary>
            public float V
            {
                get { return this._v; }
                set
                {
                    _v = value;
                }
            }

            public HsvColor(float hue, float saturation, float brightness)
            {
                if (hue < 0f || 360f <= hue)
                {
                    throw new ArgumentException(
                        "hueは0以上360未満の値です。", "hue");
                }
                if (saturation < 0f || 1f < saturation)
                {
                    throw new ArgumentException(
                        "saturationは0以上1以下の値です。", "saturation");
                }
                if (brightness < 0f || 1f < brightness)
                {
                    throw new ArgumentException(
                        "brightnessは0以上1以下の値です。", "brightness");
                }

                this._h = hue;
                this._s = saturation;
                this._v = brightness;
            }

            /// <summary>
            /// 指定したColorからHsvColorを作成する
            /// </summary>
            /// <param name="rgb">Color</param>
            /// <returns>HsvColor</returns>
            public static HsvColor FromRgb(Color rgb)
            {
                float r = (float)rgb.R / 255f;
                float g = (float)rgb.G / 255f;
                float b = (float)rgb.B / 255f;

                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(g, b));

                float brightness = max;

                float hue, saturation;
                if (max == min)
                {
                    //undefined
                    hue = 0f;
                    saturation = 0f;
                }
                else
                {
                    float c = max - min;

                    if (max == r)
                    {
                        hue = (g - b) / c;
                    }
                    else if (max == g)
                    {
                        hue = (b - r) / c + 2f;
                    }
                    else
                    {
                        hue = (r - g) / c + 4f;
                    }
                    hue *= 60f;
                    if (hue < 0f)
                    {
                        hue += 360f;
                    }

                    saturation = c / max;
                }

                return new HsvColor(hue, saturation, brightness);
            }

            /// <summary>
            /// 指定したHsvColorからColorを作成する
            /// </summary>
            /// <param name="hsv">HsvColor</param>
            /// <returns>Color</returns>
            public static Color ToRgb(HsvColor hsv)
            {
                float v = hsv.V;
                float s = hsv.S;

                float r, g, b;
                if (s == 0)
                {
                    r = v;
                    g = v;
                    b = v;
                }
                else
                {
                    float h = hsv.H / 60f;
                    int i = (int)Math.Floor(h);
                    float f = h - i;
                    float p = v * (1f - s);
                    float q;
                    if (i % 2 == 0)
                    {
                        //t
                        q = v * (1f - (1f - f) * s);
                    }
                    else
                    {
                        q = v * (1f - f * s);
                    }

                    switch (i)
                    {
                        case 0:
                            r = v;
                            g = q;
                            b = p;
                            break;
                        case 1:
                            r = q;
                            g = v;
                            b = p;
                            break;
                        case 2:
                            r = p;
                            g = v;
                            b = q;
                            break;
                        case 3:
                            r = p;
                            g = q;
                            b = v;
                            break;
                        case 4:
                            r = q;
                            g = p;
                            b = v;
                            break;
                        case 5:
                            r = v;
                            g = p;
                            b = q;
                            break;
                        default:
                            throw new ArgumentException(
                                "色相の値が不正です。", "hsv");
                    }
                }

                return Color.FromArgb(
                    (byte)255,
                    (byte)Math.Round(r * 255f),
                    (byte)Math.Round(g * 255f),
                    (byte)Math.Round(b * 255f));
            }
        }
}
