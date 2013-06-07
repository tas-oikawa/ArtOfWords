using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon;
using System.Windows;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System.Threading;
using System.Windows.Threading;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using System.Windows.Shapes;
using WritersBattleField.View;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using System.Windows.Documents;
using System.ComponentModel;
using Chokanbar.Model;
using WritersBattleField.ViewModel.MarkingLayer;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using Microsoft.Win32;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using System.Windows.Controls;
using ModernizedAlice.IPlugin.ModuleInterface;

namespace WritersBattleField.ViewModel
{
    public class WritersBattleFieldViewModel : INotifyPropertyChanged
    {
        private bool _hadFirstInitialize = false;

        private WritersBattleFieldView _writersBFView;
        private MarkingLayerView _markingLayerView;
        private MarkingSelectorViewModel _markingReviewerViewModel;
        public  MarkingLayerViewModelBase CurrentMarkingLayerViewModel;

        public MarkerViewModel MarkerModel { set; get; }

        private bool _doShowMarkReviewer;
        public bool DoShowMarkReviewer
        {
            set
            {
                if (_doShowMarkReviewer != value)
                {
                    _markingReviewerViewModel.SelectTab = 0;
                    _doShowMarkReviewer = value;
                    OnPropertyChanged("DoShowMarkReviewer");
                }
            }
            get
            {
                return _doShowMarkReviewer;
            }
        }


        private Mark _currentSelectingMark;
        public Mark CurrentSelectingMark
        {
            set
            {
                if (_currentSelectingMark != value)
                {
                    _currentSelectingMark = value;
                    OnPropertyChanged("CurrentSelectingMark");
                }
            }
            get
            {
                return _currentSelectingMark;
            }
        }

        public void SetTextToModelsComposite()
        {
            ModelsComposite.DocumentModel.Text = _writersBFView.Editor.GetText();            
        }

        public void Initialize(WritersBattleFieldView view)
        {
            _doShowMarkReviewer = false;
            _mode = ViewMode.Writing;

            if (_hadFirstInitialize)
            {
                return;
            }

            MarkerModel = new MarkerViewModel();
            _markingReviewerViewModel = new MarkingSelectorViewModel();
            CurrentMarkingLayerViewModel = new NullMarkingLayerViewModel();

            // View設定
            _writersBFView = view;
            _markingLayerView = view.GetMarkingLayerView();


            _markingReviewerViewModel.SetView(_writersBFView.MarkReviewer, _writersBFView.Editor);

            // Binding
            _writersBFView.BindModel(this);
            _markingLayerView.BindModel(this);

            CurrentMarkingLayerViewModel.SetView(_markingLayerView);
            CurrentMarkingLayerViewModel.WritersModel = this;

            // Event
            EventAggregator.MoveDocumentIndexEventRised += OnMoveDocumentIndex;
            EventAggregator.ReplaceEventRised += OnReplaceWordEvent;

            _hadFirstInitialize = true;
        }

        private bool DoUseMarkingLayer()
        {
            if (_mode == ViewMode.Character)
            {
                return true;
            } 
            else if (_mode == ViewMode.StoryFrame)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private ViewMode _mode;
        public ViewMode Mode
        {
            set
            {
                if (_mode != value)
                {
                    _writersBFView.Editor.OnModeChanged();

                    _mode = value;
                    CurrentMarkingLayerViewModel.ResetStatus();
                    if (DoUseMarkingLayer())
                    {
                        _writersBFView.ResetMarkTab();
                        switch (_mode)
                        {
                            case ViewMode.Character:
                                CurrentMarkingLayerViewModel = new TalkMarkingLayerViewModel();
                                break;
                            case ViewMode.StoryFrame:
                                CurrentMarkingLayerViewModel = new StoryFrameMarkingLayerViewModel();
                                break;
                            default:
                                break;
                        }

                        CurrentMarkingLayerViewModel.WritersModel = this;
                        CurrentMarkingLayerViewModel.SetView(_markingLayerView);
                        CurrentMarkingLayerViewModel.SetRedrawTimer();

                        DoShowMarkReviewer = false;
                    }
                    OnPropertyChanged("Mode");
                }
            }

            get
            {
                return _mode;
            }
        }

        public String Text
        {
            get
            {
                return ModelsComposite.DocumentModel.Text;
            }
            set
            {
                ModelsComposite.DocumentModel.Text = Text;
            }
        }

        public ICollection<IMarkable> GetMarkablesOnMode()
        {
            if (Mode == ViewMode.Character)
            {
                List<IMarkable> markable = new List<IMarkable>();
                foreach (var mark in ModelsComposite.CharacterManager.ModelCollection)
                {
                    markable.Add(mark as IMarkable);
                }
                return markable;
            }

            if (Mode == ViewMode.StoryFrame)
            {
                List<IMarkable> markable = new List<IMarkable>();
                foreach (var mark in ModelsComposite.StoryFrameModelManager.ModelCollection)
                {
                    markable.Add(mark as IMarkable);
                }
                return markable;
            }
            return null;
        }

        public void PrepareForMark()
        {
            var head = GetHeadIndexOfVisibleText();
            var tail = GetTailIndexOfVisibleText();

            if (head == -1 || tail == -1)
            {
                return;
            }

            ModelsComposite.MarkManager.FocusOnRange(
                head,
                tail,
                GetMarkKindEnum());
        }

        public void OnTextRegionChanged()
        {
            CurrentMarkingLayerViewModel.SetRedrawTimer();
        }

        public void OnEditor_TextSearchOccured(object sender, TextSearchEventArgs arg)
        {
            if (Mode != ViewMode.Writing)
            {
                return;
            }

            DoShowMarkReviewer = true; 
            _markingReviewerViewModel.SelectTab = 0;
            _markingReviewerViewModel.SetKeyword(arg.SearchWord);
        }

        public void MoveLineAt(int line)
        {
            int headCharIdx = GetHeadIndexOfVisibleText();
            int tailCharIdx = GetTailIndexOfVisibleText();

            if (headCharIdx == -1 || tailCharIdx == -1)
            {
                return;
            }

            int visHeadLine = _writersBFView.GetLineIndexFromCharacterIndex(headCharIdx);
            int visTailLine = _writersBFView.GetLineIndexFromCharacterIndex(tailCharIdx);

            if (line >= visHeadLine && line <= visTailLine)
            {
                return;
            }

            if (line < visHeadLine)
            {
                int upCount = visHeadLine - line;
                for (int curidx = 0; curidx < upCount; curidx++)
                {
                    _writersBFView.LineUp();
                }
            }

            if (line > visTailLine)
            {
                int downCount = line - visTailLine;
                for (int curidx = 0; curidx < downCount; curidx++)
                {
                    _writersBFView.LineDown();
                }
            }
        }

        private void OnMoveDocumentIndex(object obj, MoveDocumentIndexEventArgs args)
        {
            if(_writersBFView.Editor.GetText().Length < args.headIndex)
            {
                return ;
            }

            int line = _writersBFView.GetLineIndexFromCharacterIndex(args.headIndex);

            MoveLineAt(line);

            if (_writersBFView.Editor.GetText().Length > args.headIndex + args.targetText.Length)
            {
                string str = _writersBFView.Editor.GetText().Substring(args.headIndex, args.targetText.Length);
                if (args.targetText.Equals(str))
                {
                    _writersBFView.Editor.Select(args.headIndex, args.targetText.Length);
                    _writersBFView.Editor.Focus();
                }
            }
            
        }

        private void ReplaceOneWord(string from, int index, bool doDelete)
        {
            _writersBFView.Editor.Select(index, from.Length);
            if (doDelete)
            {
                _writersBFView.Editor.Cut();
            }
            else
            {
                _writersBFView.Editor.Paste();
            }
        }


        private void OnReplaceWordEvent(object sender, ReplaceWordEventArgs arg)
        {
            if(arg.fromStr.Length == 0)
            {
                return ;
            }

            if (arg.replaceAll)
            {
                var obj = Clipboard.GetText();
                Clipboard.SetText(arg.toStr);

                int idx = 0;
                while (true)
                {
                    idx = _writersBFView.Editor.GetText().IndexOf(arg.fromStr, idx);
                    if (idx == -1)
                    {
                        break;
                    }

                    if (arg.toStr.Length == 0)
                    {
                        ReplaceOneWord(arg.fromStr, idx, true);
                    }
                    else
                    {
                        ReplaceOneWord(arg.fromStr, idx, false);
                    }
                    idx += arg.toStr.Length;
                }

                Clipboard.SetText(obj);
            }
            else
            {
                var obj = Clipboard.GetText();
                Clipboard.SetText(arg.toStr);

                var idx = _writersBFView.Editor.GetText().IndexOf(arg.fromStr, arg.headIndex);
                if (idx == arg.headIndex)
                {
                    if (arg.toStr.Length == 0)
                    {
                        ReplaceOneWord(arg.fromStr, idx, true);
                    }
                    else
                    {
                        ReplaceOneWord(arg.fromStr, idx, false);
                    }
                }

                Clipboard.SetText(obj);
            }
        }

        #region util

        private MarkKindEnums GetMarkKindEnum()
        {
            switch(_mode)
            {
                case ViewMode.Character:
                    return MarkKindEnums.Character;
                    
                case ViewMode.StoryFrame:
                    return MarkKindEnums.StoryFrame;
                case ViewMode.Writing:
                     /* FALLTHROUGH */
                default:
                    return MarkKindEnums.None;
            }
        }

        public int GetHeadIndexOfVisibleText()
        {
            return _writersBFView.GetHeadIndexOfVisibleText();
        }

        public int GetTailIndexOfVisibleText()
        {
            return _writersBFView.GetTailIndexOfVisibleText();
        }

        public List<Rect> GetRectOfCharIndex(int headIndex, int tailIndex)
        {
            return _writersBFView.GetRectByCharIndex(headIndex, tailIndex);
        }

        public int GetTailIndexOfLineByIndex(int index)
        {
            return _writersBFView.GetTailIndexOfLineByIndex(index);
        }

        public Mark GetMarkFromIndex(int index)
        {
            return ModelsComposite.MarkManager.GetPriorityMark(index);
        }

        public int GetIndexFromPosition(Point pos)
        {
            return _writersBFView.GetIndexFromPosition(pos);
        }

        public Mark GetMarkFromPosition(Point point)
        {
            var index = _writersBFView.GetIndexFromPosition(point);

            if (index == -1)
            {
                return null;
            }

            return GetMarkFromIndex(index);
        }

        public bool AddMarkAt(Point pos)
        {
            // 現在選択中のマーカーを取得する
            IMarkable mark = MarkerModel.GetSelectingMark();

            if (mark == null)
            {
                return false;
            }
            int selectingIndex = GetIndexFromPosition(pos);
            
            if (selectingIndex == -1)
            {
                return false;
            }

            if (GetMarkFromIndex(selectingIndex) != null)
            {
                return false;
            }

            return MarkFactory.AddMark(_writersBFView.Editor.GetText(), GetIndexFromPosition(pos), mark);
        }

        public void DeleteMark(Mark mark)
        {
            ModelsComposite.MarkManager.DeleteMark(mark);
        }

        private IEnumerable<Mark> GetSelectingMarks()
        {
            List<Mark> marklist = new List<Mark>();

            IMarkable markable = MarkerModel.GetSelectingMark();

            if (markable == null)
            {
                return marklist;
            }

            return ModelsComposite.MarkManager.GetMarks(markable);
        }

        public void OnWindowActivated()
        {
            if (_writersBFView == null)
            {
                return;
            }

            _writersBFView.OnWindowActivated();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
