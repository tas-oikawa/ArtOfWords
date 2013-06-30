using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommonControls
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class CommonLightBox : Window
    {
        public enum CommonLightBoxKind
        {
            CancelOnly,
            SaveCancel,
            SaveOnly,
        }

        private CommonLightBoxKind _lightBoxKind = CommonLightBoxKind.SaveOnly;

        public CommonLightBoxKind LightBoxKind
        {
            get
            {
                return _lightBoxKind;
            }
            set
            {
                _lightBoxKind = value;
            }
        }

        private bool _isStretchable = true;

        public bool IsStretchable
        {
            get { return _isStretchable; }
            set { _isStretchable = value; }
        }


        private ScrollBarVisibility _horizontalScrollBarVisibility = ScrollBarVisibility.Auto;

        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return _horizontalScrollBarVisibility; }
            set { _horizontalScrollBarVisibility = value; }
        }
        private ScrollBarVisibility _verticalScrollBarVisibility = ScrollBarVisibility.Auto;

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return _verticalScrollBarVisibility; }
            set { _verticalScrollBarVisibility = value; }
        }

        public delegate void SaveAndQuitHandler(object sender, SaveAndQuitEventArgs e);
        public event SaveAndQuitHandler OnSaveAndQuit;

        public CommonLightBox()
        {
            InitializeComponent();
        }

        public void BindUIElement(UIElement elem)
        {
            TargetPanel.Children.Clear();
            TargetPanel.Children.Add(elem);
        }

        public void OnMouseDownAtSaveBlackoutBorder(object obj, RoutedEventArgs arg)
        {
            SaveAndQuitEventArgs quitArg = new SaveAndQuitEventArgs();
            quitArg.canClose = true;
            if (OnSaveAndQuit != null)
            {
                OnSaveAndQuit(this, quitArg);
            }

            if (quitArg.canClose)
            {
                DialogResult = true;
            }
        }

        public void OnMouseDownCancelAtBlackoutBorder(object obj, RoutedEventArgs arg)
        {
            DialogResult = false;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        // Make sure RECT is actually OUR defined struct, not the windows rect.
        public static RECT GetWindowRectangle(Window window)
        {
            RECT rect;
            GetWindowRect((new WindowInteropHelper(window)).Handle, out rect);

            return rect;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_lightBoxKind == CommonLightBoxKind.SaveCancel)
            {
                Grid.SetRowSpan(this.UpperBorder, 1);
                Grid.SetRow(this.LowerBorder, 1);
                Grid.SetRowSpan(this.LowerBorder, 2);
                this.UpperBorder.Visibility = System.Windows.Visibility.Visible;
            }
            else if (_lightBoxKind == CommonLightBoxKind.SaveOnly)
            {
                Grid.SetRow(this.UpperBorder, 0);
                Grid.SetRowSpan(this.UpperBorder, 3);
                Grid.SetRow(this.LowerBorder, 0);
                Grid.SetRowSpan(this.LowerBorder, 1);
                this.LowerBorder.Visibility = System.Windows.Visibility.Hidden;
                this.UpperBorder.Style = BaseGrid.FindResource("SaveBlackoutBorder") as Style;
                this.UpperBorderTextBlock.Style = BaseGrid.FindResource("SaveTextBlockStyle") as Style;
            }
            else if (_lightBoxKind == CommonLightBoxKind.CancelOnly)
            {
                Grid.SetRow(this.UpperBorder, 0);
                Grid.SetRowSpan(this.UpperBorder, 3);
                Grid.SetRow(this.LowerBorder, 0);
                Grid.SetRowSpan(this.LowerBorder, 1);
                this.LowerBorder.Visibility = System.Windows.Visibility.Hidden;
                this.UpperBorder.Style = BaseGrid.FindResource("CancelBlackoutBorder") as Style;
                this.UpperBorderTextBlock.Style = BaseGrid.FindResource("CancelTextBlockStyle") as Style;
            }

            if (Owner.WindowState == System.Windows.WindowState.Maximized)
            {
                var rect = GetWindowRectangle(Owner);

                this.Left = rect.Left;
                this.Top = rect.Top;
            }
            else
            {
                this.Left = Owner.Left;
                this.Top = Owner.Top;
            }

            if (!_isStretchable)
            {
                BaseGrid.RowDefinitions[0].Height = new GridLength(20);
                BaseGrid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Star);
                BaseGrid.RowDefinitions[2].Height = new GridLength(20);
            }

            this.ScrollViewer.HorizontalScrollBarVisibility = this.HorizontalScrollBarVisibility;
            this.ScrollViewer.VerticalScrollBarVisibility = this.VerticalScrollBarVisibility;

            this.Width = Owner.ActualWidth;
            this.Height = Owner.ActualHeight;

            this.TargetGrid.MaxWidth =
                Math.Max(this.Width -
                (BaseGrid.ColumnDefinitions[0].MinWidth +
                    BaseGrid.ColumnDefinitions[2].MinWidth), 0);
            this.TargetGrid.MaxHeight =
                Math.Max(this.Height -
                (BaseGrid.RowDefinitions[0].MinHeight +
                    BaseGrid.RowDefinitions[2].MinHeight), 0);
        }

        private void Window_Initialized_1(object sender, EventArgs e)
        {
        }
    }
}
