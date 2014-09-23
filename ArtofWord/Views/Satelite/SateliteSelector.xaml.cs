using ArtOfWords.Models.Satelite.CharacterSatelite;
using ArtOfWords.Models.Satelite.ItemSatelite;
using ArtOfWords.Models.Satelite.StoryFrameSatelite;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArtOfWords.Views.Satelite
{
    /// <summary>
    /// SateliteSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class SateliteSelector : UserControl
    {
        public SateliteSelector()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var context = (sender as Control).DataContext;

            var charaModel = context as CharacterModel;

            if (charaModel != null)
            {
                var view = CharacterSateliteGenerator.Generate(charaModel);

                view.Show();
                return ;
            }

            var storyModel = context as StoryFrameModel;

            if (storyModel != null)
            {
                var view = StoryFrameSateliteGenerator.Generate(storyModel);

                view.Show();
                return;
            }

            var itemModel = context as ItemModel;

            if (itemModel != null)
            {
                var view = ItemSateliteGenerator.Generate(itemModel);

                view.Show();
                return;
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            CharacterListView.ItemsSource = ModelsComposite.CharacterManager.ModelCollection;
            ItemListView.ItemsSource = ModelsComposite.ItemModelManager.ModelCollection;
            StoryFrameListView.ItemsSource = ModelsComposite.StoryFrameModelManager.ModelCollection;
        }
    }
}
