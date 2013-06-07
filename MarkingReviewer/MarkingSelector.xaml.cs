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
using Chokanbar.Model;
using SearchControl;

namespace Chokanbar
{
    /// <summary>
    /// MarkingSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class MarkingSelector : UserControl
    {
        public MarkingSelector()
        {
            InitializeComponent();
        }

        private MarkingSelectorViewModel _model;

        public void BindData(MarkingSelectorViewModel model)
        {
            _model = model;
        }

        private void TalkExtractButton_Click(object sender, RoutedEventArgs e)
        {
            _model.ExtractTalks();
        }

        private void ItemExtractButton_Click(object sender, RoutedEventArgs e)
        {
            _model.ExtractItems();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var txt = KeywordTextBox.Text;
            
            if (txt == null || txt.Count() == 0)
            {
                return;
            }

            _model.ClearChildlen();
            _model.ExtractKeyword(txt,new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)));
        }

        public void KeyExtract(string txt)
        {

            if (txt == null || txt.Count() == 0)
            {
                return;
            }

            _model.ClearChildlen();
            _model.ExtractKeyword(txt, new SolidColorBrush(Color.FromArgb(255, 200, 200, 200)));
        }

        private void KeyExtractButton_Click(object sender, RoutedEventArgs e)
        {
            KeyExtract(ReplaceFromTextBox.Text);
        }

        private void AllReplaceButton_Click(object sender, RoutedEventArgs e)
        {
            _model.ReplaceAll(ReplaceFromTextBox.Text);
        }

        private void SearchTextBox_Search_1(object sender, RoutedEventArgs e)
        {
            if (sender is SearchTextBox)
            {
                _model.SearchWord = ((SearchTextBox)sender).Text;
            }
        }
    }
}
