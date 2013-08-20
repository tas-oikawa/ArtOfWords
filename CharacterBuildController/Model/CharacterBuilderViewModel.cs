using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using CommonControls.Model;
using System.Windows.Media;
using TagsGrooveControls.Strategy;
using TagsGrooveControls.Util;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using CommonControls;
using System.Linq.Expressions;
using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;

namespace CharacterBuildControll.Model
{
    /// <summary>
    /// 入力中のページタイプを示す
    /// </summary>
    public enum PageType
    {
        /// <summary>
        /// 基本情報
        /// </summary>
        Base,

        /// <summary>
        /// 深層心理
        /// </summary>
        DeepPsyche,
    }

    public class CharacterBuildViewModel : NotifyPropertyChangedBase
    {
        private CharacterBuilder _view;

        private ObservableCollection<IMarkable> _characterModelCollection;
        public ObservableCollection<IMarkable> CharacterModelCollection
        {
            get
            {
                return _characterModelCollection;
            }
        }


        private int _selectingIndex;
        public int SelectingIndex
        {
            set
            {
                if (value != _selectingIndex)
                {
                    _selectingIndex = value;

                    GenerateTagPanel();

                    this.OnPropertyChanged(o => SelectingIndex);
                    this.OnPropertyChanged(o => SelectingModel);
                }
            }
            get
            {
                return _selectingIndex;
            }
        }

        public CharacterModel SelectingModel
        {
            get
            {
                if (SelectingIndex >= CharacterModelCollection.Count || SelectingIndex < 0)
                {
                    return null;
                }

                return CharacterModelCollection.ElementAt(SelectingIndex) as CharacterModel;
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
                if (_searchWord != value)
                {
                    _searchWord = value;
                    GetFilteredCollection();
                }
            }
        }

        private PageType _pageType = PageType.Base;

        /// <summary>
        /// 基本情報のページが開かれているかどうか
        /// </summary>
        public bool IsBasePageOpening
        {
            set
            {
                if (_pageType != PageType.Base)
                {
                    _pageType = PageType.Base;
                    OnPagePropertyChanged();
                }
            }
            get
            {
                return _pageType == PageType.Base;
            }
        }

        /// <summary>
        /// 人物分析のページが開かれているかどうか
        /// </summary>
        public bool IsDeepPsychePageOpening
        {
            set
            {
                if (_pageType != PageType.DeepPsyche)
                {
                    _pageType = PageType.DeepPsyche;
                    OnPagePropertyChanged();
                }
            }
            get
            {
                return _pageType == PageType.DeepPsyche;
            }
        }

       

        public DeletableLabelStackPanel DeletableLabelStackPanel
        {
            get
            {
                return (_view._baseControl.Content as BaseControl).TagDeletableStackPanel;
            }
        }

        public CharacterBuildViewModel()
        {
            _characterModelCollection = new ObservableCollection<IMarkable>();
            SelectingIndex = -1;

            EventAggregator.AddIMarkableHandler += AddIMarkableHandler;
            EventAggregator.DeleteIMarkableHandler += DeleteIMarkableHandler;
            EventAggregator.SelectObjectForceHandler += EventAggregator_SelectObjectForceHandler;
        }

        private void AddIMarkableHandler(object sender, ModernizedAlice.ArtOfWords.BizCommon.Model.Event.AddIMarkableModelEventArgs arg)
        {
            if (arg.Markable is CharacterModel)
            {
                SearchWord = "";
                GetFilteredCollection();
            }
        }

        private void DeleteIMarkableHandler(object sender, ModernizedAlice.ArtOfWords.BizCommon.Model.Event.DeleteIMarkableModelEventArgs arg)
        {
            if (arg.Markable is CharacterModel)
            {
                int prevIndex = SelectingIndex;
                CharacterModelCollection.Remove(arg.Markable);
                Select(prevIndex);
            }
        }

        private void EventAggregator_SelectObjectForceHandler(object sender, SelectObjectForceEventArgs e)
        {
            if (!(e.Model is CharacterModel))
            {
                return;
            }
            var chara = e.Model as CharacterModel;

            Select(chara);
        }

        public void GetFilteredCollection()
        {
            var prevModel = SelectingModel;
            int prevIndex = SelectingIndex;

            _characterModelCollection.Clear();

            foreach (var model in ModelsComposite.CharacterManager.ModelCollection)
            {
                _characterModelCollection.Add(model);
            }

            SearchWordUtil.DoFilterMark(SearchWord, _characterModelCollection);

            if (_characterModelCollection.Contains(prevModel))
            {
                Select(prevModel);
            }
            else
            {
                Select(SelectingIndex);
            }
        }

        public void Initialize(CharacterBuilder view)
        {
            _view = view;

            _searchWord = "";
            _view.SearchWordBox.Text = "";
            GetFilteredCollection();

            _view.BindData(this);
            if (CharacterModelCollection.Count() >= 1)
            {
                SelectingIndex = -1;
                SelectingIndex = 0;
            }
            else
            {
                SelectingIndex = -1;
            }
        }

        public void Select(int index)
        {
            int selectIndex;
            if (CharacterModelCollection.Count() > index)
            {
                selectIndex = index;
            }
            else
            {
                selectIndex = CharacterModelCollection.Count() - 1;
            }
            SelectingIndex = selectIndex;
        }

        public void Select(CharacterModel model)
        {
            int selectIndex = -1;
            if(model != null)
            {
                selectIndex = CharacterModelCollection.IndexOf(model);
            }

            SelectingIndex = selectIndex;
        }

        public void MoveCollection(int idx1, int idx2)
        {
            var collection = ModelsComposite.CharacterManager.ModelCollection;

            var model1 = CharacterModelCollection.ElementAt(idx1);
            var model2 = CharacterModelCollection.ElementAt(idx2);

            var moveFrom = collection.IndexOf(model1);
            var moveTo = collection.IndexOf(model2);

            collection.Move(moveFrom, moveTo);

            CharacterModelCollection.Move(idx1, idx2);
        }

        #region Tagコントロール

        private void GenerateTagPanel()
        {
            if (SelectingModel == null)
            {
                return;
            }

            DeletableLabelStackPanel.NoItemMessage = "ここには貼り付けたタグが表示されます";
            DeletableLabelStackPanel.DoNotShowAddButtonIfCountZero = false;
            DeletableLabelStackPanel.DoShowNoItemErrorMessageIfCountZero = false;
            DeletableLabelStackPanel.DataList = 
                TagToAppearListViewModelConverter.ToAppearListViewItemModel(SelectingModel.Tags,ModelsComposite.TagManager);
            DeletableLabelStackPanel.AddButtonStrategy = new TagStickerAddButtonStrategy(SelectingModel, ModelsComposite.TagManager);
            DeletableLabelStackPanel.OnModelIsAppearedChangedEvent += TagDeletableStackPanel_OnModelIsAppearedChangedEvent;
            DeletableLabelStackPanel.Initialize();
        }

        void TagDeletableStackPanel_OnModelIsAppearedChangedEvent(object sender, CommonControls.OnModelIsAppearedChangedEventArgs e)
        {
            if (SelectingModel == null)
            {
                return;
            }

            var lvItem = sender as AppearListViewItemModel;

            var chara = lvItem.ParentObject as TagModel;
            if (chara != null)
            {
                ChangeIsAppearedTag(chara, e.IsAppeared);
                return;
            }
        }

        private void ChangeIsAppearedTag(TagModel tag, bool isAppeared)
        {
            if (isAppeared == false)
            {
                SelectingModel.Tags.Remove(tag.Id);
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPagePropertyChanged()
        {
            this.OnPropertyChanged(o => IsBasePageOpening);
            this.OnPropertyChanged(o => IsDeepPsychePageOpening);
        }

        #endregion
    }
}
