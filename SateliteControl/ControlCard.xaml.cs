using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SateliteControl
{
    /// <summary>
    /// ControlCard.xaml の相互作用ロジック
    /// </summary>
    public partial class ControlCard : UserControl
    {
        #region DependencyProperty

        private UIElement _topLeftGridElement;

        /// <summary>
        /// 左上のグリッドの子
        /// </summary>
        public UIElement TopLeftGridElement
        {
            get { return _topLeftGridElement; }
            set
            {
                _topLeftGridElement = value;
                TopLeftGrid.Children.Clear();
                TopLeftGrid.Children.Add(value);
            }
        }

        private UIElement _topRightGridElement;

        /// <summary>
        /// 右上のグリッドの子
        /// </summary>
        public UIElement TopRightGridElement
        {
            get { return _topRightGridElement; }
            set
            {
                _topRightGridElement =  value;
                TopRightGrid.Children.Clear();
                TopRightGrid.Children.Add(value);
            }
        }

        private UIElement _bottomLeftGridElement;

        /// <summary>
        /// 左下のグリッドの子
        /// </summary>
        public UIElement BottomLeftGridElement
        {
            get { return _bottomLeftGridElement; }
            set
            {
                _bottomLeftGridElement = value;
                BottomLeftGrid.Children.Clear();
                BottomLeftGrid.Children.Add(value);
            }
        }

        private UIElement _bottomRightGridElement;

        /// <summary>
        /// 右下のグリッドの子
        /// </summary>
        public UIElement BottomRightGridElement
        {
            get { return _bottomRightGridElement; }
            set
            {
                _bottomRightGridElement = value;
                BottomRightGrid.Children.Clear();
                BottomRightGrid.Children.Add(value);
            }
        }

        #endregion

        public ControlCard()
        {
            InitializeComponent();
        }

        public void SetRotateMode()
        {
            ((RotateTransform)TopLeftGrid.RenderTransform).Angle = 0;
            ((RotateTransform)TopRightGrid.RenderTransform).Angle = 90;
            ((RotateTransform)BottomRightGrid.RenderTransform).Angle = 180;
            ((RotateTransform)BottomLeftGrid.RenderTransform).Angle = 270;
        }

        public void SetSquareMode()
        {
            ((RotateTransform)TopLeftGrid.RenderTransform).Angle = 0;
            ((RotateTransform)TopRightGrid.RenderTransform).Angle = 0;
            ((RotateTransform)BottomRightGrid.RenderTransform).Angle = 0;
            ((RotateTransform)BottomLeftGrid.RenderTransform).Angle = 0;
        }
    }
}
