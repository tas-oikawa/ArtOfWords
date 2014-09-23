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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArtOfWords.About
{
    /// <summary>
    /// AboutBoxWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutBoxWindow : Window
    {
        public AboutBoxWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.kanazawa-net.ne.jp/~pmansato/");
            }
            catch { }
        }

        private void Hyperlink_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://main.tinyjoker.net/");
            }
            catch { }
        }

        private void Hyperlink_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://davidowens.wordpress.com/");
            }
            catch { }

        }
    }
}
