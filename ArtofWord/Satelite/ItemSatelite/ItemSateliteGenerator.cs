using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using SateliteControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ArtOfWords.Satelite.ItemSatelite
{
    public static class ItemSateliteGenerator
    {
        public static SateliteViewer Generate(ItemModel model)
        {
            var viewer = new SateliteViewer(Application.Current.MainWindow);
            var viewModel = new ItemSateliteViewModel(viewer, model);

            viewer.LeftButtonLabel = "ステータス";
            viewer.TopButtonLabel = "自由記入欄";
            viewer.RightButtonLabel = "未使用";
            viewer.BottomButtonLabel = "未使用";

            viewer.RelatedModel = model;
            viewer.TopLeftGridElement = new StatusGrid() { DataContext = viewModel };
            viewer.TopRightGridElement = new RemarkGrid() { DataContext = viewModel };
            viewer.BottomRightGridElement = new NotUsedGrid() { DataContext = viewModel };
            viewer.BottomLeftGridElement = new NotUsedGrid() { DataContext = viewModel };

            return viewer;
        }
    }
}
