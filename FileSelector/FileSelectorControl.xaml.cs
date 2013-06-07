using CommonControls.Util;
using FileSelector.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace FileSelector
{
    /// <summary>
    /// Interaction logic for FileSelectorControl.xaml
    /// </summary>
    public partial class FileSelectorControl : Window
    {
        private FileSelectorViewModel _viewModel;

        public String FilePath;

        public FileSelectorControl()
        {
            InitializeComponent();
        }

        public void SetViewModel(FileSelectorViewModel viewModel)
        {
            _viewModel = viewModel;

            this.DataContext = _viewModel;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _viewModel.OpenFileDialog();
        }

        private void GridViewColumnHeader_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFileEventOccured()
        {
            FilePath = _viewModel.OpenFilePath;

            if (!File.Exists(FilePath))
            {
                ShowDialogManager.ShowMessageBox("ファイルが開けません。正しいファイルを指定してください。", "ファイルがありません", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileEventOccured();
        }

        private void ListView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedRecentlyUsed != null)
            {
                OpenFileEventOccured();
            }
        }

        private void ListView_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.RecentlyUsedFiles != null)
            {
                OpenFileEventOccured();
            }
        }

        private void ListBox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedNovelsBox != null)
            {
                OpenFileEventOccured();
            }
        }
    }
}
