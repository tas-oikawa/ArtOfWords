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
using WritersBattleField.ViewModel;

namespace WritersBattleField.View
{
    /// <summary>
    /// PageLineCalculatorView.xaml の相互作用ロジック
    /// </summary>
    public partial class PageLineCalculatorView : UserControl
    {
        private PageLineCalculatorViewModel _model;

        public PageLineCalculatorView()
        {
            InitializeComponent();
        }


        public void BindModel(PageLineCalculatorViewModel model)
        {
            _model = model;
            this.DataContext = _model;
        }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);

        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_model != null)
            {
                _model.CalculateLineAndPages();
            }
        }
    }
}
