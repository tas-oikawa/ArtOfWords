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
using CommonControls;
using CommonControls.Util;

namespace WritersBattleField.ViewModel
{
    /// <summary>
    /// 執筆空間のViewModel
    /// </summary>
    public class WritersBattleFieldViewModel : INotifyPropertyChanged
    {
        #region Variables
        private bool _hadFirstInitialize = false;

        /// <summary>
        /// 執筆View
        /// </summary>
        private WritersBattleFieldView _writersBFView;

        /// <summary>
        /// マーキング用View
        /// </summary>
        private MarkingLayerView _markingLayerView;

        /// <summary>
        /// みつけるあつめる用ViewModel
        /// </summary>
        private MarkingSelectorViewModel _markingReviewerViewModel;

        #endregion

        #region Properties
        /// <summary>
        /// マーキングレイヤーのViewModel
        /// </summary>
        public  MarkingLayerViewModelBase CurrentMarkingLayerViewModel;


        /// <summary>
        /// マーカーのViewModel(ユーザーが選択しているマーカーを示す-ある登場人物とか）
        /// </summary>
        public MarkerViewModel MarkerModel { set; get; }

        private bool _doShowMarkReviewer;

        /// <summary>
        /// みつけるあつめる画面を表示するかどうか
        /// </summary>
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
        /// <summary>
        /// 現在選択中のマーク
        /// </summary>
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

        private ViewMode _mode = ViewMode.Writing;
        /// <summary>
        /// 執筆画面の表示モード
        /// </summary>
        public ViewMode Mode
        {
            set
            {
                if (_mode == value)
                {
                    return;
                }

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

            get
            {
                return _mode;
            }
        }

        /// <summary>
        /// 執筆しているTEXT。ModelsCompositeへのアクセスを提供しているだけ
        /// </summary>
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
        #endregion

        /// <summary>
        /// 執筆TextBoxに書かれたTextをModelsCompositeに設定する
        /// </summary>
        /// <remarks>
        /// このようにBindingせずに明示的にModelと分離しているのは、
        /// パフォーマンスの理由による
        /// </remarks>
        public void SetTextToModelsComposite()
        {
            ModelsComposite.DocumentModel.Text = _writersBFView.Editor.GetText();            
        }

        /// <summary>
        /// 執筆Viewを初期化する
        /// </summary>
        /// <param name="view">対応するビュー</param>
        public void Initialize(WritersBattleFieldView view)
        {
            _doShowMarkReviewer = false;
            _mode = ViewMode.Writing;

            // 一度Initializeしていたらここまでの処理だけでOK
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

        /// <summary>
        /// マーキングレイヤーを表示するモードかどうか判定する
        /// </summary>
        /// <returns>true:マーキングレイヤーを表示。false:表示しない</returns>
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

        /// <summary>
        /// マーキング可能なオブジェクトのコレクションを返す
        /// </summary>
        /// <returns>マーキング可能なオブジェクトのコレクション</returns>
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

        /// <summary>
        /// マークを描画するための準備をします（高速化のためのキャッシュ化）
        /// </summary>
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

        /// <summary>
        /// テキスト領域がスクロールされたときのイベント
        /// </summary>
        public void OnTextRegionScrolled()
        {
            CurrentMarkingLayerViewModel.SetRedrawTimer();
        }

        /// <summary>
        /// テキストサーチが始まったときのイベント
        /// </summary>
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

        /// <summary>
        /// 行を移動する
        /// </summary>
        /// <param name="line">移動先の行番号</param>
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

        /// <summary>
        /// ページ数を計算する
        /// </summary>
        public void CalculatePageNumber()
        {
            SetTextToModelsComposite();
            CommonLightBox dialog = new CommonLightBox();

            var pageLineCalcView = new PageLineCalculatorView();
            var pageLineCalculatorViewModel = new PageLineCalculatorViewModel(ModelsComposite.NovelSettingModel, ModelsComposite.DocumentModel);

            pageLineCalcView.BindModel(pageLineCalculatorViewModel);

            dialog.Owner = Application.Current.MainWindow;
            dialog.BindUIElement(pageLineCalcView);
            dialog.LightBoxKind = CommonLightBox.CommonLightBoxKind.SaveOnly;

            ShowDialogManager.ShowDialog(dialog);
        }
        
        /// <summary>
        /// 検索とか置換でドキュメントが変化したときのイベント
        /// </summary>
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

        /// <summary>
        /// テキストを置換する
        /// </summary>
        /// <param name="from">置換するワード</param>
        /// <param name="index">置換開始位置</param>
        /// <param name="doDelete">削除か貼り付けか</param>
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

        /// <summary>
        /// 置換が発生した時のイベント
        /// </summary>
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

        /// <summary>
        /// ModeからMarkKindを取得する
        /// </summary>
        /// <returns>MarkKind</returns>
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

        /// <summary>
        /// 表示されている領域の中で先頭の文字Indexを取得する
        /// </summary>
        /// <returns>先頭の文字Index</returns>
        public int GetHeadIndexOfVisibleText()
        {
            return _writersBFView.GetHeadIndexOfVisibleText();
        }

        /// <summary>
        /// 表示されている領域の中で末尾の文字Indexを取得する
        /// </summary>
        /// <returns>末尾の文字Index</returns>
        public int GetTailIndexOfVisibleText()
        {
            return _writersBFView.GetTailIndexOfVisibleText();
        }

        /// <summary>
        /// 文字Indexの範囲を含むRectを取得する
        /// </summary>
        /// <param name="headIndex">先頭Index</param>
        /// <param name="tailIndex">末尾Index</param>
        /// <returns>RectのList</returns>
        /// <remarks>文字の途中で改行されることもあるのでList型。二行に渡る場合は二個のRectが入る</remarks>
        public List<Rect> GetRectOfCharIndex(int headIndex, int tailIndex)
        {
            return _writersBFView.GetRectByCharIndex(headIndex, tailIndex);
        }

        /// <summary>
        /// ある文字が存在する行の末尾の文字Indexを取得する
        /// </summary>
        /// <param name="index">文字Index</param>
        /// <returns>行の末尾の文字Index</returns>
        public int GetTailIndexOfLineByIndex(int index)
        {
            return _writersBFView.GetTailIndexOfLineByIndex(index);
        }

        /// <summary>
        /// 文字Indexからそれに対応するMarkを取得する
        /// </summary>
        /// <param name="index">文字Index</param>
        /// <returns>Mark</returns>
        public Mark GetMarkFromIndex(int index)
        {
            return ModelsComposite.MarkManager.GetPriorityMark(index);
        }

        /// <summary>
        /// X、Y座標から文字Indexを取得する
        /// </summary>
        /// <param name="pos">X、Y座標</param>
        /// <returns>文字Index</returns>
        public int GetIndexFromPosition(Point pos)
        {
            return _writersBFView.GetIndexFromPosition(pos);
        }

        /// <summary>
        /// X,Y座標からMarkを取得する
        /// </summary>
        /// <param name="point">X、Y座標</param>
        /// <returns>Mark</returns>
        public Mark GetMarkFromPosition(Point point)
        {
            var index = _writersBFView.GetIndexFromPosition(point);

            if (index == -1)
            {
                return null;
            }

            return GetMarkFromIndex(index);
        }

        /// <summary>
        /// X,Y座標にMarkを追加する
        /// </summary>
        /// <param name="pos">X,Y座標</param>
        /// <returns>処理の正否</returns>
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

        /// <summary>
        /// Markを削除する
        /// </summary>
        /// <param name="mark">削除するMark</param>
        public void DeleteMark(Mark mark)
        {
            ModelsComposite.MarkManager.DeleteMark(mark);
        }

        /// <summary>
        /// WindowがActivateしたときのイベント
        /// </summary>
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
