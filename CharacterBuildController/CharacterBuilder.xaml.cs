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
using CharacterBuildControll.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using System.Collections.ObjectModel;
using CommonControls.Util;
using CommonControls;
using TagsGrooveControls.View;
using TagsGrooveControls.Model;
using ModernizedAlice.ArtOfWords.Services.ModelService;

namespace CharacterBuildControll
{
    /// <summary>
    /// CharacterBuildControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CharacterBuilder : UserControl
    {
        private CharacterBuildViewModel _model;

        public CharacterBuilder()
        {
            InitializeComponent();
        }

        public void BindData(CharacterBuildViewModel model)
        {
            _model = model;

            this.DataContext = _model;
        }


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            this.SearchWordBox.Text = "";
            var characterService = new CharacterModelService();

            var newModel = characterService.AddNewCharacter();
            _model.Select(newModel);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_model.SelectingModel == null)
            {
                return;
            }
            var manager = ModelsComposite.CharacterManager;

            if (ShowDialogManager.ShowMessageBox("ほんとうに削除してもいいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var service = new CharacterModelService();
                service.RemoveCharacter(_model.SelectingModel);
            }
        }

        private void colorbutton_Click(object sender, RoutedEventArgs e)
        {
            var userControl = new GeneralColorPicker();
            CommonLightBox lightBox = new CommonLightBox();
            lightBox.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveCancel;
            lightBox.BindUIElement(userControl);
            lightBox.Owner = Application.Current.MainWindow;

            userControl.SelectingColor = ((SolidColorBrush)_model.SelectingModel.ColorBrush).Color;

            if (ShowDialogManager.ShowDialog(lightBox) == true)
            {
                _model.SelectingModel.ColorBrush = new SolidColorBrush(userControl.SelectingColor);
            }
        }

        #region Drag And Drop 管理
        ListBoxItem dragItem;
        Point dragStartPos;
        DragAdorner dragGhost;

        private void listBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // マウスダウンされたアイテムを記憶
            dragItem = sender as ListBoxItem;
            // マウスダウン時の座標を取得
            dragStartPos = e.GetPosition(dragItem);
        }
        private void listBoxItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var lbi = sender as ListBoxItem;
            if (e.LeftButton == MouseButtonState.Pressed && dragGhost == null && dragItem == lbi)
            {
                var nowPos = e.GetPosition(lbi);
                if (Math.Abs(nowPos.X - dragStartPos.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(nowPos.Y - dragStartPos.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    charaListBox.AllowDrop = true;

                    var layer = AdornerLayer.GetAdornerLayer(charaListBox);
                    dragGhost = new DragAdorner(charaListBox, lbi, 0.5, dragStartPos);
                    layer.Add(dragGhost);
                    DragDrop.DoDragDrop(lbi, lbi, DragDropEffects.Move);
                    layer.Remove(dragGhost);
                    dragGhost = null;
                    dragItem = null;

                    charaListBox.AllowDrop = false;
                }
            }
        }
        private void listBoxItem_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (dragGhost != null)
            {
                var p = NativeMethods.GetNowPosition(this);
                var loc = this.PointFromScreen(charaListBox.PointToScreen(new Point(0, 0)));
                dragGhost.LeftOffset = p.X - loc.X;
                dragGhost.TopOffset = p.Y - loc.Y;
            }
        }
        private void listBox_Drop(object sender, DragEventArgs e)
        {
            var dropPos = e.GetPosition(charaListBox);
            var lbi = e.Data.GetData(typeof(ListBoxItem)) as ListBoxItem;
            var o = lbi.DataContext as IMarkable;
            var index = _model.CharacterModelCollection.IndexOf(o);
            for (int i = 0; i <  _model.CharacterModelCollection.Count; i++)
            {
                var item = charaListBox.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;
                var pos = charaListBox.PointFromScreen(item.PointToScreen(new Point(0, item.ActualHeight / 2)));
                if (dropPos.Y < pos.Y)
                {
                    // i が入れ換え先のインデックス
                    _model.MoveCollection(index, (index < i) ? i - 1 : i);
                    return;
                }
            }
            // 最後にもっていく
            int last = _model.CharacterModelCollection.Count - 1;
            _model.MoveCollection(index, last);
        }
        #endregion

        private void SearchTextBox_Search_1(object sender, RoutedEventArgs e)
        {
            _model.SearchWord = this.SearchWordBox.Text;
        }

        private void ExecuteDelete(object sender, ExecutedRoutedEventArgs e)
        {
            deleteButton_Click(sender, e);
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (_model.SelectingIndex < 1)
            {
                return;
            }

            _model.MoveCollection(_model.SelectingIndex, _model.SelectingIndex - 1);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (_model.SelectingIndex < 0 || _model.SelectingIndex + 1 >= _model.CharacterModelCollection.Count())
            {
                return;
            }

            _model.MoveCollection(_model.SelectingIndex, _model.SelectingIndex + 1);
        }
    }
}
