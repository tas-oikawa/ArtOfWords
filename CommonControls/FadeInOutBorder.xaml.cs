using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace CommonControls
{
    /// <summary>
    /// FadeInOutBorder.xaml の相互作用ロジック
    /// </summary>
    public partial class FadeInOutBorder : ContentControl
    {
        public bool IsInvisible
        {
            get { return (bool)GetValue(IsInvisibleProperty); }
            set 
            { 
                SetValue(IsInvisibleProperty, value);
            }
        }
        public static readonly DependencyProperty IsInvisibleProperty =
            DependencyProperty.Register("IsInvisible", typeof(bool), typeof(FadeInOutBorder), new UIPropertyMetadata(false, new PropertyChangedCallback(OnIsInvisibleChanged)));
        
        // 依存プロパティが変更されたときの処理
        private static void OnIsInvisibleChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            FadeInOutBorder userControl = obj as FadeInOutBorder;

            if (userControl != null)
            {
                if ((bool)e.NewValue == true)
                {
                    userControl.BeginStoryboard(userControl.GetFadeOutStoryboard());
                }
                else
                {
                    userControl.BeginStoryboard(userControl.GetFadeInStoryboard());
                }
            }
        }

        public FadeInOutBorder()
        {
            InitializeComponent();

        }

        public Storyboard GetFadeInStoryboard()
        {
            return (Storyboard)this.FindResource("FadeInStoryBoard");
        }

        public Storyboard GetFadeOutStoryboard()
        {
            return (Storyboard)this.FindResource("FadeOutStoryBoard");
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInvisible)
            {
                this.Opacity = 0;
                this.RenderTransform = new TranslateTransform(-20.0, 0);
            }
            else
            {
                this.Opacity = 1;
                this.RenderTransform = new TranslateTransform(0, 0);
            }
        }
    }
}
