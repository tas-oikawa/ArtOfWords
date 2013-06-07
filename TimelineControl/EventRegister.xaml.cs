using CommonControls;
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
    /// EventRegister.xaml の相互作用ロジック
    /// </summary>
    public partial class EventRegister : UserControl
    {
        private EventViewModel _model;

        public EventRegister()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            _model = this.DataContext as EventViewModel;

            this.DeletableStackPanel.NoItemMessage = "ここには登録した登場人物が表示されます";
            this.DeletableStackPanel.DataList = _model.AppearListViewItems;
            this.DeletableStackPanel.Initialize();
        }

        public void OnSaveAndQuit(object sender, SaveAndQuitEventArgs e)
        {
            e.canClose = _model.IsSavable();
        }
    }
}
