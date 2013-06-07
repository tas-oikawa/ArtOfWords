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

namespace TagsGrooveControls
{
    /// <summary>
    /// TagSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class TagSelector : UserControl
    {
        private TagSelectorViewModel _model;

        public TagSelector()
        {
            InitializeComponent();
        }

        public void SetModel(TagSelectorViewModel model)
        {
            _model = model;
            _model.SetView(this);
            this.DataContext = _model;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _model.AddSelectedTag();
        }

        private void _deleteChildButton_Click(object sender, RoutedEventArgs e)
        {
            var listItem = sender as FrameworkElement;

            _model.RemoveTag(listItem.DataContext as TagListItemModel);
        }
    }
}
