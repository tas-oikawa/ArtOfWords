using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CommonControls.Util
{
    public class ColorUtil
    {
        public static Color Darker(Color org)
        {
            double f = 0.9;

            var newR = (1.0 - f) * org.R + f * 0;
            var newG = (1.0 - f) * org.G + f * 0;
            var newB = (1.0 - f) * org.B + f * 0;

            return Color.FromArgb(org.A, (byte)newR, (byte)newG, (byte)newB);
        }

        public static Color Lighter(Color org, double rate)
        {
            var newR = (1.0 - rate) * org.R + rate * 255;
            var newG = (1.0 - rate) * org.G + rate * 255;
            var newB = (1.0 - rate) * org.B + rate * 255;

            return Color.FromArgb(org.A, (byte)newR, (byte)newG, (byte)newB);
        }

        public static Color Lighter(Color org)
        {
            return Lighter(org, 0.9);
        }

        public static Color ExtremelyLighter(Color org)
        {
            return Lighter(org, 0.995);
        }
    }
}
