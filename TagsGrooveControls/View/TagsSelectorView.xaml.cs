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
    /// TagsSelectorView.xaml の相互作用ロジック
    /// </summary>
    public partial class TagsSelectorView : UserControl
    {
        TagSelectorViewModel _model;

        public TagsSelectorView()
        {
            InitializeComponent();
        }

        public void SetModel(TagSelectorViewModel model)
        {
            _model = model;

            _model.SetView(this._treeView);
            this.DataContext = _model;
        }

        private void addSelectButton_Click(object sender, RoutedEventArgs e)
        {
            _model.AddSelection();
        }

        private void _deleteChildButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (sender as FrameworkElement).DataContext as TagModel;

            _model.RemoveSelection(model);
        }
    }
}
