using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System.Windows.Controls;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using ModernizedAlice.ArtOfWords.BizCommon;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using System.ComponentModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.IPlugin.ModuleInterface;

namespace Chokanbar.Model
{
    public class MarkingSelectorViewModel : INotifyPropertyChanged
    {
        private MarkingSelector _view;
        private IEditor _iEditor;

        private List<MarkReview> _markReviewList;
        public ObservableCollection<IMarkable> TalkingDataList
        {
            get
            {
                var filteredCollection = new ObservableCollection<IMarkable>(ModelsComposite.CharacterManager.ModelCollection);

                SearchWordUtil.DoFilterMark(SearchWord, filteredCollection);

                return filteredCollection;
            }
        }

        public ObservableCollection<IMarkable> ItemDataList
        {
            get
            {
                var filteredCollection = new ObservableCollection<IMarkable>(ModelsComposite.ItemModelManager.ModelCollection);

                SearchWordUtil.DoFilterMark(SearchWord, filteredCollection);

                return filteredCollection;
            }
        }

        public ObservableCollection<IMarkable> StoryFrameDataList
        {
            get
            {
                return ModelsComposite.StoryFrameModelManager.ModelCollection;
            }
        }

        private string _searchWord;
        public string SearchWord
        {
            get
            {
                return _searchWord;
            }
            set
            {
                if (value != _searchWord)
                {
                    _searchWord = value;
                    OnPropertyChanged("TalkingDataList");
                    OnPropertyChanged("ItemDataList");
                }
            }
        }

        private int _selectTab;
        public int SelectTab
        {
            set
            {
                if (_selectTab != value)
                {
                    _searchWord = "";
                    _selectTab = value;

                    PropertyChanged(this, new PropertyChangedEventArgs("SelectTab"));

                    // まずはクリア
                    _view.MainBoardStack.Children.Clear();

                    if (_selectTab == 4)
                    {
                        ExtractStoryFrames();
                    }
                }
            }
            get
            {
                return _selectTab;
            }
        }

        public void SetView(MarkingSelector view, IEditor iEditor)
        {
            _selectTab = 0;
            _iEditor = iEditor;

            _view = view;
            _view.DataContext = this;

            _view.BindData(this);

            EventAggregator.AddIMarkableHandler += EventAggregator_AddIMarkableHandler;
            EventAggregator.DeleteIMarkableHandler += EventAggregator_DeleteIMarkableHandler;
        }

        private void EventAggregator_DeleteIMarkableHandler(object sender, DeleteIMarkableModelEventArgs arg)
        {
            OnPropertyChanged("TalkingDataList");
            OnPropertyChanged("ItemDataList");
            ClearChildlen();
        }

        private void EventAggregator_AddIMarkableHandler(object sender, AddIMarkableModelEventArgs arg)
        {
            OnPropertyChanged("TalkingDataList");
            OnPropertyChanged("ItemDataList");
            ClearChildlen();
        }

        private IOrderedEnumerable<Mark> GenerateOrderedEnumerable(IEnumerable<Mark> markCollection)
        {
            var marks = from mrk in markCollection
                        orderby mrk.HeadCharIndex
                        select mrk;

            return marks;
        }

        private Control GetAddControl(Mark mark, String document)
        {
            MarkReview review  = new MarkReview(mark,document);
            MarkReviewControl control = new MarkReviewControl();

            control.BindData(this);

            _markReviewList.Add(review);

            control.DataContext = review;

            return control;
        }

        public void ShowMarks(IEnumerable<Mark> markCollection, String document)
        {
            _markReviewList = new List<MarkReview>();

            var markList = GenerateOrderedEnumerable(markCollection);

            foreach (var elem in markList)
            {
                _view.MainBoardStack.Children.Add(GetAddControl(elem, document));
            }
        }

        public void ExtractTalks()
        {
            // キャラを選択する
            var items = _view.TalkListBox.SelectedItems;
            var marks = new List<Mark>();

            foreach(var item in items)
            {
                var chara = item as CharacterModel;

                marks.AddRange(ModelsComposite.MarkManager.GetMarks(chara));
            }

            ClearChildlen();
            ShowMarks(marks, _iEditor.GetText());
        }


        const int KeyWordRange = 30;

        private int GetHeadIndex(int headIndex, string str)
        {
            int retIndex = headIndex;

            return Math.Max(0, retIndex);
        }

        private int GetTailIndex(int tailIndex, string str)
        {
            int retIndex = tailIndex + KeyWordRange;

            return Math.Min(str.Length - 1 , retIndex);
        }

        public void ClearChildlen()
        {
            _view.MainBoardStack.Children.Clear();
        }

        public void ExtractKeyword(string keyword, Brush brush)
        {
            var marks = new List<Mark>();

            var txt = _iEditor.GetText();

            int currentIndex = 0;

            while (true)
            {
                currentIndex = txt.IndexOf(keyword, currentIndex);
                if (currentIndex == -1)
                {
                    break;
                }

                int tailIndex =  currentIndex + keyword.Length - 1;


                marks.Add(new KeywordMark()
                {
                    Brush = brush,
                    HeadCharIndex = GetHeadIndex(currentIndex, txt),
                    TailCharIndex = GetTailIndex(tailIndex, txt),
                    Parent = null,
                    keyword = keyword
                });
                                          
                currentIndex = tailIndex + 1;
                if (currentIndex >= txt.Length)
                {
                    break;
                }
            }

            ShowMarks(marks, _iEditor.GetText());
        }

        public void ExtractItems()
        {
            // アイテムを選択する
            var items = _view.ItemListBox.SelectedItems;
            var marks = new List<Mark>();

            ClearChildlen();
            foreach (var sel in items)
            {
                var item = sel as ItemModel;

                ExtractKeyword(item.Name, item.ColorBrush);
            }
        }

        public void ExtractStoryFrames()
        {
            var marks = new List<Mark>();
            foreach (var mark in ModelsComposite.MarkManager.GetMarks())
            {
                if (mark.GetType() == typeof(StoryFrameMark))
                {
                    marks.Add(mark);
                }
            }

            ClearChildlen();
            ShowMarks(marks, _iEditor.GetText());
        }

        public void ReplaceAll(string moveText)
        {
            EventAggregator.OnReplaceWordEvent(this, new ReplaceWordEventArgs()
            {
                replaceAll = true,
                fromStr = moveText,
                toStr = _view.ReplaceToTextBox.Text
            });
        }
        public void JumpButtonPushed(int moveIndex, string moveText)
        {
            if (_view.tabControl1.SelectedItem == _view.replaceTabItem)
            {
                EventAggregator.OnReplaceWordEvent(this, new ReplaceWordEventArgs()
                {
                    headIndex = moveIndex,
                    tailIndex = moveIndex + moveText.Length - 1,
                    replaceAll = false,
                    fromStr = moveText,
                    toStr = _view.ReplaceToTextBox.Text
                });

                _view.KeyExtract(moveText);
            }
            else
            {
                EventAggregator.OnMoveDocumentIndex(this, new MoveDocumentIndexEventArgs() { headIndex = moveIndex, targetText = moveText });
            }
        }

        public void SetKeyword(string key)
        {
            _view.KeywordTextBox.Text = key;
        }

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
