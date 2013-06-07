using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WritersBattleField.View;
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System.Windows.Media.Imaging;

namespace WritersBattleField.ViewModel.MarkingLayer
{
    public abstract class MarkingLayerViewModelBase
    {
        private int noRedrawCount = 0;
        private DispatcherTimer redrawTimer;
        private DateTime nextDrawTime;

        protected Button _deleteButton;

        public MarkingLayerView _view;

        public WritersBattleFieldViewModel WritersModel { set; get; }

        public void SetView(MarkingLayerView view)
        {
            _view = view;
        }

        protected virtual void DeleteButtonClicked(object obj, RoutedEventArgs e)
        {
        }

        public void RedrawClock(object sender, EventArgs e)
        {
            if (nextDrawTime > DateTime.Now)
            {
                return;
            }

            redrawTimer.Stop();
            redrawTimer = null;

            if (_view.Visibility == Visibility.Visible)
            {
                RedrawMarks();
            }
        }

        public void SetRedrawTimer()
        {
            if (noRedrawCount < 20)
            {
                nextDrawTime = DateTime.Now.AddMilliseconds(10);
            }
            if (redrawTimer == null)
            {
                noRedrawCount = 0;
                redrawTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 10), DispatcherPriority.Normal, new EventHandler(RedrawClock), Dispatcher.CurrentDispatcher);
                redrawTimer.Start();
            }
            else
            {
                noRedrawCount++;
            }
        }

        protected void DrawRectangle(Rect rect, Brush brush, Mark mark)
        {
            if (rect.X < 0)
            {
                rect.Width += rect.X;
                rect.X = 0;
            }
            if (rect.Y < 0)
            {
                rect.Height += rect.Y;
                rect.Y = 0;
            }
            
            Rectangle rectangle = new Rectangle();
            rectangle.Width = rect.Width;
            rectangle.Height = rect.Height;
            rectangle.Fill = brush;

            if (mark != null)
            {
                rectangle.ContextMenu = new System.Windows.Controls.ContextMenu();

                var menuItem = new MenuItem() { Header = "削除" };

                menuItem.Click += DeleteMenuItemClicked;
                menuItem.DataContext = mark;

                rectangle.ContextMenu.Items.Add(menuItem);
            }

            Canvas.SetLeft(rectangle, rect.Left);
            Canvas.SetTop(rectangle, rect.Top);
            _view.canvas.Children.Add(rectangle);
        }


        private void DeleteMenuItemClicked(object obj, RoutedEventArgs e)
        {
            var item = obj as MenuItem;
            var mark = item.DataContext as Mark;

            this.WritersModel.DeleteMark(mark);

            RedrawMarks();
        }

        protected void RedrawAllMarks()
        {
            WritersModel.PrepareForMark();
            int head = WritersModel.GetHeadIndexOfVisibleText();
            int tail = WritersModel.GetTailIndexOfVisibleText();

            if (head == -1 || tail == -1)
            {
                return;
            }

            for (int currentIndex = head; currentIndex <= tail; ++currentIndex)
            {
                var mark = WritersModel.GetMarkFromIndex(currentIndex);
                if (mark == null)
                {
                    continue;
                }
                DrawOneMark(mark.HeadCharIndex, mark.TailCharIndex, mark.Brush, mark);
                currentIndex = mark.TailCharIndex;
            }
        }

        protected void DrawOneMark(int headIndex, int tailIndex, Brush brush, Mark mark)
        {
            int visibleHead = WritersModel.GetHeadIndexOfVisibleText();
            int visibleTail = WritersModel.GetTailIndexOfVisibleText();

            if (visibleHead == -1 || visibleTail == -1)
            {
                return;
            }

            int headOfRect = (headIndex > visibleHead) ? headIndex : visibleHead;
            int tailOfRect = (tailIndex < visibleTail) ? tailIndex : visibleTail;

            var DrawRect = WritersModel.GetRectOfCharIndex(headOfRect, tailOfRect);

            foreach (var rect in DrawRect)
            {
                if (rect.Height < 0 || rect.Width < 0)
                {
                    continue;
                }

                DrawRectangle(rect, brush, mark);
            }
        }

        public abstract void RedrawMarks();
        public abstract void ResetStatus();

        public virtual void ExecuteMouseMoveAsHovering(Point point)
        {

        }

        public virtual void OnCanvasMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                ResetStatus();
            }
            else
            {
                if (WritersModel.AddMarkAt(e.GetPosition(_view.canvas)))
                {
                    RedrawMarks();
                }
            }
        }

        protected void DrawDeleteButton(int index)
        {
            var rect = WritersModel.GetRectOfCharIndex(index, index);
            _deleteButton = new Button();

            _deleteButton.Margin = new Thickness(0);
            _deleteButton.Padding = new Thickness(0);

            var height = 20;
            if (rect[0].Top - height < 0)
            {
                Canvas.SetTop(_deleteButton, rect[0].Bottom);
            }
            else
            {
                Canvas.SetTop(_deleteButton, rect[0].Top - height);
            }
            Canvas.SetLeft(_deleteButton, rect[0].Right - 23);

            _deleteButton.FontSize = 9;
            _deleteButton.Width = 23;
            _deleteButton.Height = Double.NaN;

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(@"/WritersBattleField;component/Images/CloseButton.png", UriKind.Relative);
            image.EndInit();

            _deleteButton.Content = new Image() { Source = image, Stretch = System.Windows.Media.Stretch.UniformToFill };
            _deleteButton.Click += DeleteButtonClicked;

            _view.canvas.Children.Add(_deleteButton);
        }

        public virtual void OnCanvasMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
        }
    }
}
