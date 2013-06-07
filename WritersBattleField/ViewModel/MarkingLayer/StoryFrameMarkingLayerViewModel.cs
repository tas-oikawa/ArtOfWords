using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WritersBattleField.ViewModel.MarkingLayer
{
    public class StoryFrameMarkingLayerViewModel : MarkingLayerViewModelBase
    {

        public override void ResetStatus()
        {
            CurrentHoveringMark = null;
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
                var headLineTailPos = WritersModel.GetTailIndexOfLineByIndex(CurrentHoveringMark.HeadCharIndex);
                DrawDeleteButton(Math.Min(headLineTailPos, CurrentHoveringMark.TailCharIndex));
            }
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
        #endregion


        public override void RedrawMarks()
        {
            _view.ClearUIElement(typeof(Rectangle));
            RedrawAllMarks();
        }

        public override void OnCanvasMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point curPos = e.GetPosition(_view);

            ExecuteMouseMoveAsHovering(curPos);
        }

        public override void ExecuteMouseMoveAsHovering(Point point)
        {
            if (_deleteButton != null && _deleteButton.IsMouseOver)
            {
                return;
            }

            var mark = WritersModel.GetMarkFromPosition(point);

            CurrentHoveringMark = mark;
        }
    }
}
