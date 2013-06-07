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

namespace Chokanbar
{
    /// <summary>
    /// MarkReviewControl.xaml の相互作用ロジック
    /// </summary>
    public partial class MarkReviewControl : UserControl
    {
        private MarkingSelectorViewModel _model;

        public MarkReviewControl()
        {
            InitializeComponent();
        }

        public void BindData(MarkingSelectorViewModel model)
        {
            _model = model;
        }

        private void JumpButtonClicked(object sender, RoutedEventArgs e)
        {
            var markReview = this.DataContext as MarkReview;

            _model.JumpButtonPushed(markReview.HeadIndex, markReview.GetKeywordText());
        }
    }
}
