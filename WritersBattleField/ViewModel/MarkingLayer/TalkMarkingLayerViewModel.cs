using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace WritersBattleField.ViewModel.MarkingLayer
{
    public class TalkMarkingLayerViewModel : MarkingLayerViewModelBase
    {

        private void RangeDirectionButtonClicked(object obj, RoutedEventArgs e)
        {
            if (obj == _rightRangeButton)
            {
                _rangeDirection = RangeDirection.ToRight;
            }
            else
            {
                _rangeDirection = RangeDirection.ToLeft;
            }

            _currentSelectingMark = _currentHoveringMark;
            _selectingMarkForDraw = new Mark(_currentSelectingMark);
            _currentHoveringMark = null;

            _view.ClearUIElement(typeof(Button));
            SetRedrawTimer();
        }

        protected override void DeleteButtonClicked(object obj, RoutedEventArgs e)
        {
            if (_currentHoveringMark == null)
            {
                return;
            }
            this.WritersModel.DeleteMark(_currentHoveringMark);

            RedrawMarks();
            ResetStatus();
        }

        public override void ResetStatus()
        {
            CurrentHoveringMark = null;
            CurrentSelectingMark = null;
            SelectingMarkForDraw = null;
        }

        private void DrawSelectionButton(int index, bool toLeft)
        {
            var rect = WritersModel.GetRectOfCharIndex(index, index);

            Button button = new Button();

            button.Margin = new Thickness(0);
            button.Padding = new Thickness(0);
            if (toLeft)
            {
                _leftRangeButton = button;
                _leftRangeButton.Click += RangeDirectionButtonClicked;
            }
            else
            {
                _rightRangeButton = button;
                _rightRangeButton.Click += RangeDirectionButtonClicked;
            }

            if (toLeft)
                Canvas.SetLeft(button, rect[0].Left - 20);
            else
                Canvas.SetLeft(button, rect[0].Left + rect[0].Width -10);
            Canvas.SetTop(button, rect[0].Top - 3);

            button.FontSize = 9;
            button.Width = 23;
            button.Height = rect[0].Height + 3;

            button.Content = (toLeft) ? "←" : "→";

            _view.canvas.Children.Add(button);
        }


        private void OnHoveringMarkChanged()
        {
            _view.ClearUIElement(typeof(Button));
            if (CurrentHoveringMark == null)
            {
                return;
            }

            int head = WritersModel.GetHeadIndexOfVisibleText();
            int tail = WritersModel.GetTailIndexOfVisibleText();

            if (head == -1 || tail == -1)
            {
                return;
            }

            if (head <= CurrentHoveringMark.HeadCharIndex)
            {
                DrawSelectionButton(CurrentHoveringMark.HeadCharIndex, true);

                var headLineTailPos = WritersModel.GetTailIndexOfLineByIndex(CurrentHoveringMark.HeadCharIndex);
                DrawDeleteButton(Math.Min(headLineTailPos, CurrentHoveringMark.TailCharIndex));

            }
            if (tail >= CurrentHoveringMark.TailCharIndex)
            {
                DrawSelectionButton(CurrentHoveringMark.TailCharIndex, false);
            }
        }



        private void RedrawSelection()
        {
            DrawOneMark(_selectingMarkForDraw.HeadCharIndex, _selectingMarkForDraw.TailCharIndex, _selectingMarkForDraw.Brush, null);
        }

        public override void RedrawMarks()
        {
            _view.ClearUIElement(typeof(Rectangle));
            if (Mode == MarkLayerMode.RangeSetting)
            {
                RedrawSelection();
            }
            else
            {
                RedrawAllMarks();
            }
        }

        public override void OnCanvasMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                ResetStatus();
            }
            else
            {
                if (Mode == MarkLayerMode.None)
                {
                    if (WritersModel.AddMarkAt(e.GetPosition(_view.canvas)))
                    {
                        RedrawMarks();
                    }
                }
                else
                {
                    FixMarkRange(e.GetPosition(_view.canvas));
                }
            }
        }

        public override void OnCanvasMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point curPos = e.GetPosition(_view);

            if (Mode == MarkLayerMode.None)
            {
                ExecuteMouseMoveAsHovering(curPos);
            }
            else if (Mode == MarkLayerMode.RangeSetting)
            {
                ExecuteMouseMoveAsSelecting(curPos);
            }
        }

        private void FixMarkRange(Point pos)
        {
            // カレントポジションをインデックスに変更
            int index = WritersModel.GetIndexFromPosition(pos);

            if (index == -1)
            {
                return;
            }

            int headIndex = 0;
            int tailIndex = 0;
            GetTailAndHead(out headIndex, out tailIndex, index);

            // Markのレンジを変更
            CurrentSelectingMark.HeadCharIndex = headIndex;
            CurrentSelectingMark.TailCharIndex = tailIndex;

            // カレントを戻す
            CurrentSelectingMark = null;

            // ReDraw
            SetRedrawTimer();
        }

        public override void ExecuteMouseMoveAsHovering(Point point)
        {
            if (_leftRangeButton != null && _leftRangeButton.IsMouseOver)
            {
                return;
            }
            if (_rightRangeButton != null && _rightRangeButton.IsMouseOver)
            {
                return;
            }
            if (_deleteButton != null && _deleteButton.IsMouseOver)
            {
                return;
            }

            var mark = WritersModel.GetMarkFromPosition(point);

            CurrentHoveringMark = mark;
        }

        private void ExecuteMouseMoveAsSelecting(Point point)
        {
            // カレントポジションをインデックスに変更
            int index = WritersModel.GetIndexFromPosition(point);

            if (index == -1)
            {
                return;
            }

            int headIndex = 0;
            int tailIndex = 0;

            GetTailAndHead(out headIndex, out tailIndex, index);

            if (_selectingMarkForDraw.HeadCharIndex != headIndex ||
                _selectingMarkForDraw.TailCharIndex != tailIndex)
            {
                _selectingMarkForDraw.HeadCharIndex = headIndex;
                _selectingMarkForDraw.TailCharIndex = tailIndex;

                SetRedrawTimer();
            }
        }

        private void GetTailAndHead(out int headIndex, out int tailIndex, int currentIndex)
        {
            headIndex = _selectingMarkForDraw.HeadCharIndex;
            tailIndex = _selectingMarkForDraw.TailCharIndex;

            if (_rangeDirection == RangeDirection.ToLeft)
            {
                if (currentIndex > tailIndex)
                {
                    return;
                }

                headIndex = currentIndex;
            }
            else
            {
                if (currentIndex < headIndex)
                {
                    return;
                }

                tailIndex = currentIndex;
            }
        }

        #region MarkProperties

        private Mark _currentHoveringMark;
        private Mark CurrentHoveringMark
        {
            set
            {
                if (_currentHoveringMark != value)
                {
                    _currentHoveringMark = value;
                    OnHoveringMarkChanged();
                }
            }
            get
            {
                return _currentHoveringMark;
            }
        }

        private Mark _currentSelectingMark;
        private Mark CurrentSelectingMark
        {
            set
            {
                if (_currentSelectingMark != value)
                {
                    _currentSelectingMark = value;

                    if (_currentSelectingMark == null)
                    {
                        SelectingMarkForDraw = null;
                        SetRedrawTimer();
                    }
                }
            }
            get
            {
                return _currentSelectingMark;
            }
        }

        private Mark _selectingMarkForDraw;
        private Mark SelectingMarkForDraw
        {
            set
            {
                if (_selectingMarkForDraw != value)
                {
                    _selectingMarkForDraw = value;
                }
            }
            get
            {
                return _selectingMarkForDraw;
            }
        }

        private Button _leftRangeButton;
        private Button _rightRangeButton;

        enum RangeDirection
        {
            None,
            ToLeft,
            ToRight,
        }

        private RangeDirection _rangeDirection;

        private enum MarkLayerMode
        {
            None,
            RangeSetting,
        }

        private MarkLayerMode Mode
        {
            get
            {
                if (_currentSelectingMark != null)
                {
                    return MarkLayerMode.RangeSetting;
                }
                return MarkLayerMode.None;
            }
        }

        #endregion
    }
}
