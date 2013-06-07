using System.Windows;
using System.Windows.Controls;
using WritersBattleField.ViewModel;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System;
using System.Windows.Threading;
using WritersBattleField.ViewModel.MarkingLayer;

namespace WritersBattleField.View
{
    /// <summary>
    /// MarkingLayerView.xaml の相互作用ロジック
    /// </summary>
    public partial class MarkingLayerView : UserControl
    {
        public MarkingLayerView()
        {
            InitializeComponent();
        }

        public WritersBattleFieldViewModel WritersModel { set; get; }

        public void BindModel(WritersBattleFieldViewModel model)
        {
            WritersModel = model;

            DataContext = model;
        }

        public void ClearUIElement(Type uiElement)
        {
            var removeObjects = new List<UIElement>();
            foreach (var child in canvas.Children)
            {
                if (child.GetType() == uiElement)
                {
                    removeObjects.Add(child as UIElement);
                }
            }

            foreach (var obj in removeObjects)
            {
                canvas.Children.Remove(obj);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            WritersModel.CurrentMarkingLayerViewModel.SetRedrawTimer();
        }



        private void canvas_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WritersModel.CurrentMarkingLayerViewModel.OnCanvasMouseDown(sender, e);
        }


        private void canvas_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            WritersModel.CurrentMarkingLayerViewModel.OnCanvasMouseMove(sender, e);
        }
    }
}
