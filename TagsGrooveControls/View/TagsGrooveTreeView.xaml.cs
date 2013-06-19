using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
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
using TagsGrooveControls.Model;

namespace TagsGrooveControls.View
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class TagsGrooveTreeView : UserControl
    {
        public TagsGrooveTreeView()
        {
            InitializeComponent();
        }

        private void _addChildButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (sender as FrameworkElement).DataContext as TagTreeViewItemModel;

            GetModel().AddChild(model);
        }

        private void _deleteChildButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (sender as FrameworkElement).DataContext as TagTreeViewItemModel;

            if (MessageBox.Show(model.Name + "を削除してもいいですか？", "確認", MessageBoxButton.YesNo) == MessageBoxResult.No)
            {
                return;
            }

            GetModel().Remove(model);
        }

        private TagsGrooveTreeViewModel GetModel()
        {
            return this.DataContext as TagsGrooveTreeViewModel;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var model = (sender as FrameworkElement).DataContext as TagTreeViewItemModel;

            if (model.IsSelected)
            {
                model.IsNameMode = true;
            }
        }
    }
}
