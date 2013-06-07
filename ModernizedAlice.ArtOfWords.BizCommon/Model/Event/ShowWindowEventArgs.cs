using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ModernizedAlice.ArtOfWords.BizCommon.Event
{
    public enum WindowKind
    {
    }

    public class ShowWindowEventArgs : EventArgs
    {
        public WindowKind windowKind;
        public Rect targetRect;
        public Action actAfterWindowClose;
    }
}
