using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CommonControls
{
    /// <summary>
    /// Interaction logic for DeletableLabelControl.xaml
    /// </summary>
    public partial class DeletableLabelControl : UserControl, INotifyPropertyChanged
    {
        public DeletableLabelControl()
        {
            InitializeComponent();
        }

        public delegate void OnDeleteButtonPushedHandler(object sender, RoutedEventArgs e);
        public event OnDeleteButtonPushedHandler OnDeletePushed;


        public void OnButtonDeletePushed(object sender, RoutedEventArgs e)
        {
            if (OnDeletePushed != null)
            {
                OnDeletePushed(this, e);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
