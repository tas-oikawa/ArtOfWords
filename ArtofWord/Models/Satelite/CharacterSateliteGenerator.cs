using ArtOfWords.ViewModels.Satelite.CharacterSatelite;
using ArtOfWords.Views.Satelite.CharacterSatelite;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using SateliteControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArtOfWords.Models.Satelite.CharacterSatelite
{
    public static class CharacterSateliteGenerator
    {
        public static SateliteViewer Generate(CharacterModel model)
        {
            var viewer = new SateliteViewer(Application.Current.MainWindow);
            var viewModel = new CharacterSateliteViewModel(viewer, model);

            viewer.LeftButtonLabel = "名前";
            viewer.TopButtonLabel = "ステータス";
            viewer.RightButtonLabel = "自由記入欄";
            viewer.BottomButtonLabel = "未使用";

            viewer.RelatedModel = model;
            viewer.TopLeftGridElement = new NameGrid() { DataContext = viewModel };
            viewer.TopRightGridElement = new StatusGrid() { DataContext = viewModel };
            viewer.BottomRightGridElement = new RemarkGrid() { DataContext = viewModel };
            viewer.BottomLeftGridElement = new NotUsedGrid() { DataContext = viewModel };

            return viewer;
        }
    }
}
