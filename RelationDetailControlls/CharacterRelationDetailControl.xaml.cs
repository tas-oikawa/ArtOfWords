using ModernizedAlice.ArtOfWords.BizCommon.Model.Relation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RelationDetailControlls
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class CharacterRelationDetailControl : UserControl
    {
        public CharacterRelationDetailControl()
        {
            InitializeComponent();
        }

        public void BindData(Collection<CharacterStoryRelationModel> data)
        {
            CharacterRelationListView.ItemsSource = data;
        }
    }
}
