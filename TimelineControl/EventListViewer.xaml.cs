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
using TimelineControl.Model;

namespace TimelineControl
{
    /// <summary>
    /// EventListViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class EventListViewer : UserControl
    {
        public DateTime JumpDateTime;

        public EventListViewer()
        {
            InitializeComponent();
        }

        private void Button_StartClick_1(object sender, RoutedEventArgs e)
        {
            var window = VisualTreeHelper.FindAncestor<Window>(this);
            if (window != null)
            {
                JumpDateTime = ((sender as Control).DataContext as EventViewerItemViewModel).StartDateTime;

                window.DialogResult = true;
            }
        }
        private void Button_EndClick_1(object sender, RoutedEventArgs e)
        {
            var window = VisualTreeHelper.FindAncestor<Window>(this);
            if (window != null)
            {
                JumpDateTime = ((sender as Control).DataContext as EventViewerItemViewModel).EndDateTime;

                window.DialogResult = true;
            }
        }
    }
}
