using SateliteControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SateliteControl
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class SateliteViewer : Window
    {
        public enum Cards
        {
            TopLeft,
            TopRight,
            ButtomLeft,
            ButtomRight,
        }

        private SateliteViewerViewModel _model;
        private Window _parent;

        #region DependencyProperty

        public double GridSquareSize
        {
            get { return (double)GetValue(GridSquareSizeProperty); }
            set
            {
                SetValue(GridSquareSizeProperty, value);
                if (_model != null)
                {
                    _model.GridSize = value;
                }
            }

        }

        /// <summary>依存関係プロパティ本体</summary>
        public static readonly DependencyProperty GridSquareSizeProperty =
            DependencyProperty.Register("GridSquareSize", typeof(double),
                        typeof(SateliteViewer),
                        new FrameworkPropertyMetadata(200.0));

        /// <summary>
        /// 上部のボタンのラベル
        /// </summary>
        public String TopButtonLabel
        {
            get { return (String)GetValue(TopButtonLabelProperty); }
            set 
            { 
                SetValue(TopButtonLabelProperty, value);
                this.TopRightBtn.Content = value;
            }
        }

        /// <summary>依存関係プロパティ本体</summary>
        public static readonly DependencyProperty TopButtonLabelProperty =
            DependencyProperty.Register("TopButtonLabel", typeof(String),
                        typeof(SateliteViewer),
                        new FrameworkPropertyMetadata("上部"));

        /// <summary>
        /// 下部のボタンのラベル
        /// </summary>
        public String BottomButtonLabel
        {
            get { return (String)GetValue(BottomButtonLabelProperty); }
            set 
            {
                SetValue(BottomButtonLabelProperty, value);
                this.BottomLeftBtn.Content = value;
            }
        }

        /// <summary>依存関係プロパティ本体</summary>
        public static readonly DependencyProperty BottomButtonLabelProperty =
            DependencyProperty.Register("BottomButtonLabel", typeof(String),
                        typeof(SateliteViewer),
                        new FrameworkPropertyMetadata("下部"));

        /// <summary>
        /// 左部のボタンのラベル
        /// </summary>
        public String LeftButtonLabel
        {
            get { return (String)GetValue(LeftButtonLabelProperty); }
            set 
            { 
                SetValue(LeftButtonLabelProperty, value);
                this.TopLeftBtn.Content = new TextBlock() { Text = value, TextWrapping = TextWrapping.Wrap };
            }
        }

        /// <summary>依存関係プロパティ本体</summary>
        public static readonly DependencyProperty LeftButtonLabelProperty =
            DependencyProperty.Register("LeftButtonLabel", typeof(String),
                        typeof(SateliteViewer),
                        new FrameworkPropertyMetadata("左部"));

        /// <summary>
        /// 右部のボタンのラベル
        /// </summary>
        public String RightButtonLabel
        {
            get { return (String)GetValue(RightButtonLabelProperty); }
            set 
            {
                SetValue(RightButtonLabelProperty, value);
                this.BottomRightBtn.Content = new TextBlock(){Text = value, TextWrapping = TextWrapping.Wrap};
            }
        }

        /// <summary>依存関係プロパティ本体</summary>
        public static readonly DependencyProperty RightButtonLabelProperty =
            DependencyProperty.Register("RightButtonLabel", typeof(String),
                        typeof(SateliteViewer),
                        new FrameworkPropertyMetadata("右部"));


        #endregion

        public object RelatedModel;

        /// <summary>
        /// 左上のグリッドの子
        /// </summary>
        public UIElement TopLeftGridElement
        {
            get { return MyCard.TopLeftGridElement; }
            set
            {
                MyCard.TopLeftGridElement = value;
            }
        }

        /// <summary>
        /// 右上のグリッドの子
        /// </summary>
        public UIElement TopRightGridElement
        {
            get { return MyCard.TopRightGridElement; }
            set
            {
                MyCard.TopRightGridElement = value;
            }
        }

        /// <summary>
        /// 左下のグリッドの子
        /// </summary>
        public UIElement BottomLeftGridElement
        {
            get { return MyCard.BottomLeftGridElement; }
            set
            {
                MyCard.BottomLeftGridElement = value;
            }
        }

        /// <summary>
        /// 右下のグリッドの子
        /// </summary>
        public UIElement BottomRightGridElement
        {
            get { return MyCard.BottomRightGridElement; }
            set
            {
                MyCard.BottomRightGridElement = value;
            }
        }

        public delegate void OnJumpOccuredEventHandler(object sender, OnJumpOccuredEventArgs e);
        public event OnJumpOccuredEventHandler OnJumpEvent;

        public void OnJumpOccured(object sender)
        {
            if (OnJumpEvent != null)
            {
                OnJumpEvent(sender, new OnJumpOccuredEventArgs() { Model = RelatedModel });
            }
        }


        public SateliteViewer(Window Parent)
        {
            InitializeComponent();

            _parent = Parent;
            _model = new SateliteViewerViewModel(this);

            this.DataContext = _model;

            Initialize();
        }

        private void Initialize()
        {
            if (_parent != null)
            {
                _parent.Closing += OnParentClosing;
            }
            StartAnimation();
        }

        private System.Drawing.Rectangle GetParentWindowRect()
        {
            System.Drawing.Rectangle windowRectangle;

            if (_parent.WindowState == System.Windows.WindowState.Maximized)
            {
                /* Here is the magic:
                 * Use Winforms code to find the Available space on the
                 * screen that contained the window 
                 * just before it was maximized
                 * (Left, Top have their values from Normal WindowState)
                 */
                windowRectangle = System.Windows.Forms.Screen.GetWorkingArea(
                    new System.Drawing.Point((int)_parent.Left, (int)_parent.Top));
            }
            else
            {
                windowRectangle = new System.Drawing.Rectangle(
                    (int)_parent.Left, (int)_parent.Top,
                    (int)_parent.ActualWidth, (int)_parent.ActualHeight);
            }

            return windowRectangle;
        }

        private void StartAnimation()
        {
            if(_parent == null)
            {
                return ;
            }

            var parentRect = GetParentWindowRect();

            DoubleAnimation oLabelAngleAnimation = new DoubleAnimation();
            oLabelAngleAnimation.From = parentRect.Bottom;
            oLabelAngleAnimation.To = Math.Max(parentRect.Top, 0);
            oLabelAngleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            
            var ease = new PowerEase();
            ease.Power = 8;
            ease.EasingMode = EasingMode.EaseInOut;
            oLabelAngleAnimation.EasingFunction = ease;

            Storyboard.SetTarget(oLabelAngleAnimation, this);
            Storyboard.SetTargetProperty(oLabelAngleAnimation, new PropertyPath("(Window.Top)"));
            
            Storyboard board = new Storyboard();
            
            board.Children.Add(oLabelAngleAnimation);

            oLabelAngleAnimation = new DoubleAnimation();
            oLabelAngleAnimation.From = parentRect.Left;
            oLabelAngleAnimation.To = parentRect.Right - this.Width;
            oLabelAngleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            oLabelAngleAnimation.EasingFunction = ease;

            Storyboard.SetTarget(oLabelAngleAnimation, this);
            Storyboard.SetTargetProperty(oLabelAngleAnimation, new PropertyPath("(Window.Left)"));

            board.Children.Add(oLabelAngleAnimation);

            board.Begin();
        }

        private void OnParentClosing(object sender, CancelEventArgs e)
        {
            this.Close();
        }

        public double GetAngle(Cards card)
        {
            switch (card)
            {
                case Cards.TopLeft:
                    return 0;
                case Cards.ButtomLeft:
                    return 90;
                case Cards.ButtomRight:
                    return 180;
                case Cards.TopRight:
                    return 270;
            }

            return 360;
        }

        private double GetFromAngleByMinimumDistance(double toAngle)
        {
            var nativeFrom = ((RotateTransform)MyCard.RenderTransform).Angle;

            if (Math.Abs(nativeFrom - toAngle) > Math.Abs(nativeFrom - 360 - toAngle))
            {
                return nativeFrom - 360;
            }

            if (Math.Abs(nativeFrom - toAngle) > Math.Abs(nativeFrom + 360 - toAngle))
            {
                return nativeFrom + 360;
            }
            return nativeFrom;
        }

        public void RotateTo(Cards card)
        {
            double angle = GetAngle(card);
            DoubleAnimation oLabelAngleAnimation = new DoubleAnimation();
            oLabelAngleAnimation.From = GetFromAngleByMinimumDistance(angle);
            oLabelAngleAnimation.To = angle; 
            oLabelAngleAnimation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            RotateTransform oTransform = (RotateTransform)MyCard.RenderTransform;
            oTransform.BeginAnimation(RotateTransform.AngleProperty, oLabelAngleAnimation);
        }

        private void TopRightBtn_Click(object sender, RoutedEventArgs e)
        {
            RotateTo(Cards.TopRight);
        }

        private void TopLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            RotateTo(Cards.TopLeft);
        }

        private void BottomRightBtn_Click(object sender, RoutedEventArgs e)
        {
            RotateTo(Cards.ButtomRight);
        }

        private void BottomLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            RotateTo(Cards.ButtomLeft);
        }

        private void UserControl_MouseEnter_1(object sender, MouseEventArgs e)
        {
            _model.DoShowExpandControl = true;
            if (!_model.IsExpanding)
            {
                _model.DoShowRotateControl = true;
            }
        }

        private void UserControl_MouseLeave_1(object sender, MouseEventArgs e)
        {
            _model.DoShowRotateControl = false;
            if (!_model.IsExpanding)
            {
                _model.DoShowExpandControl = false;
            }
        }

        private void ExpandBtn_Click(object sender, RoutedEventArgs e)
        {
            RotateTo(Cards.TopLeft);
            _model.OnExpandButtonClicked();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MoveBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Border_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            if (_parent != null)
            {
                _parent.Closing -= OnParentClosing;
            }
        }

        private void JumpBtn_Click(object sender, RoutedEventArgs e)
        {
            OnJumpOccured(sender);
        }
    }
}
