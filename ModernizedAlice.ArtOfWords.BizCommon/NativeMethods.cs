using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace ModernizedAlice.ArtOfWords.BizCommon
{
    public static class NativeMethods
    {
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT pt);

        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        private struct POINT
        {
            public UInt32 X;
            public UInt32 Y;
        }

        public static Point GetNowPosition(Visual v)
        {
            POINT p;
            GetCursorPos(out p);

            var source = HwndSource.FromVisual(v) as HwndSource;
            var hwnd = source.Handle;

            ScreenToClient(hwnd, ref p);
            return new Point(p.X, p.Y);
        }
    }
}
