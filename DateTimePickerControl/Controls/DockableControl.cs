using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace AC.AvalonControlsLibrary.Controls
{
    /// <summary>
    /// Control that can be Docked in a DockableContainer
    /// </summary>
    public class DockableControl : HeaderedContentControl
    {
        /// <summary>
        /// Static constructor
        /// </summary>
        static DockableControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DockableControl), new FrameworkPropertyMetadata(typeof(DockableControl)
                    ));
        }
    }
}
