using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using SateliteControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArtOfWords.Satelite.StoryFrameSatelite
{
    public static class StoryFrameSateliteGenerator
    {
        public static SateliteViewer Generate(StoryFrameModel model)
        {
            var viewer = new SateliteViewer(Application.Current.MainWindow);
            var viewModel = new StoryFrameSateliteViewModel(viewer, model);

            viewer.LeftButtonLabel = "ステータス";
            viewer.TopButtonLabel = "登場人物";
            viewer.RightButtonLabel = "登場アイテム";
            viewer.BottomButtonLabel = "自由記入欄";

            viewer.RelatedModel = model;
            viewer.TopLeftGridElement = new StatusGrid() { DataContext = viewModel };
            viewer.TopRightGridElement = new CharactersGrid() { DataContext = viewModel };
            viewer.BottomRightGridElement = new ItemsGrid() { DataContext = viewModel };
            viewer.BottomLeftGridElement = new RemarkGrid() { DataContext = viewModel };

            return viewer;
        }
    }
}
