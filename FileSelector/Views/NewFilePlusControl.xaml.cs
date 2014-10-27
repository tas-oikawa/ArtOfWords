using FileSelector.ViewModels.NewFilePlus;
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

namespace FileSelector.Views
{
    /// <summary>
    /// NewFilePlusControl.xaml の相互作用ロジック
    /// </summary>
    public partial class NewFilePlusControl : Window
    {
        public NewFilePlusControl()
        {
            InitializeComponent();
        }

        private void _createNewPlusBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #region Events
        private void _allCharacterSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllCharactersSelection(true);
        }

        private void _allCharacterUnselectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllCharactersSelection(false);
        }

        private void _allItemSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllItemsSelection(true);
        }

        private void _allItemUnselectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllItemsSelection(false);
        }

        private void _allStoryFrameSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllStoryFramesSelection(true);
        }

        private void _allStoryFrameUnselectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllStoryFramesSelection(false);
        }

        private void _allEventSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllEventsSelection(true);
        }

        private void _allEventUnselectBtn_Click(object sender, RoutedEventArgs e)
        {
            GetViewModel().ChangeAllEventsSelection(false);
        }

        #endregion

        #region ViewModel取得
        /// <summary>
        /// ViewModelを取得する
        /// </summary>
        /// <returns>ViewModel</returns>
        private NewFilePlusViewModel GetViewModel()
        {
            return DataContext as NewFilePlusViewModel;
        }
        #endregion
    }
}
