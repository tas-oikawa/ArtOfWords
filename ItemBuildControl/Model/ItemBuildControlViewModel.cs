using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using TagsGrooveControls.Strategy;
using TagsGrooveControls.Util;
using CommonControls.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;

namespace ItemBuildControl.Model
{
    public class ItemBuildControlViewModel : INotifyPropertyChanged
    {
        private ItemBuildControl _view;

        private ObservableCollection<IMarkable> _itemModelCollection;
        public ObservableCollection<IMarkable> ItemModelCollection
        {
            get
            {
                return _itemModelCollection;
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
                    OnPropertyChanged("SelectingIndex");

                    OnPropertyChanged("SelectingModel");
                }
            }
            get
            {
                return _selectingIndex;
            }
        }

        public ItemModel SelectingModel
        {
            get
            {
                if (SelectingIndex >= ItemModelCollection.Count || SelectingIndex < 0)
                {
                    return null;
                }

                return ItemModelCollection.ElementAt(SelectingIndex) as ItemModel;
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

        public ItemBuildControlViewModel()
        {
            _itemModelCollection = new ObservableCollection<IMarkable>();
            SelectingIndex = -1;

            EventAggregator.AddIMarkableHandler += AddIMarkableHandler;
            EventAggregator.DeleteIMarkableHandler += DeleteIMarkableHandler;
            EventAggregator.SelectObjectForceHandler += EventAggregator_SelectObjectForceHandler;
        }

        private void AddIMarkableHandler(object sender, ModernizedAlice.ArtOfWords.BizCommon.Model.Event.AddIMarkableModelEventArgs arg)
        {
            if (arg.Markable is ItemModel)
            {
                SearchWord = "";
                GetFilteredCollection();
            }
        }

        private void DeleteIMarkableHandler(object sender, DeleteIMarkableModelEventArgs e)
        {
            if (e.Markable is ItemModel)
            {
                int prevIndex = SelectingIndex;
                ItemModelCollection.Remove(e.Markable);
                Select(prevIndex);
            }
        }

        void EventAggregator_SelectObjectForceHandler(object sender, SelectObjectForceEventArgs e)
        {
            if (!(e.Model is ItemModel))
            {
                return;
            }

            var item = e.Model as ItemModel;
            Select(item);
        }

        public void GetFilteredCollection()
        {
            var prevModel = SelectingModel;
            int prevIndex = SelectingIndex;

            _itemModelCollection.Clear();

            foreach (var model in ModelsComposite.ItemModelManager.ModelCollection)
            {
                _itemModelCollection.Add(model);
            }

            SearchWordUtil.DoFilterMark(SearchWord, _itemModelCollection);

            if (_itemModelCollection.Contains(prevModel))
            {
                Select(prevModel);
            }
            else
            {
                Select(SelectingIndex);
            }
        }

        public void Initialize(ItemBuildControl view)
        {
            _view = view;

            _searchWord = "";
            _view.SearchWordBox.Text = "";
            GetFilteredCollection();

            _view.BindData(this);

            if (ItemModelCollection.Count() >= 1)
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
            if (ItemModelCollection.Count() > index)
            {
                selectIndex = index;
            }
            else
            {
                selectIndex = ItemModelCollection.Count() - 1;
            }
            SelectingIndex = selectIndex;
        }

        public void Select(ItemModel model)
        {
            int selectIndex = -1;
            if (model != null)
            {
                selectIndex = ItemModelCollection.IndexOf(model);
            }

            SelectingIndex = selectIndex;
        }

        public void MoveCollection(int idx1, int idx2)
        {
            var collection = ModelsComposite.ItemModelManager.ModelCollection;

            var model1 = ItemModelCollection.ElementAt(idx1);
            var model2 = ItemModelCollection.ElementAt(idx2);

            var moveFrom = collection.IndexOf(model1);
            var moveTo = collection.IndexOf(model2);

            collection.Move(moveFrom, moveTo);

            ItemModelCollection.Move(idx1, idx2);
        }


        #region Tagコントロール

        private void GenerateTagPanel()
        {
            if (SelectingModel == null)
            {
                return;
            }

            _view.TagDeletableStackPanel.NoItemMessage = "ここには貼り付けたタグが表示されます";
            _view.TagDeletableStackPanel.DoNotShowAddButtonIfCountZero = false;
            _view.TagDeletableStackPanel.DoShowNoItemErrorMessageIfCountZero = false;
            _view.TagDeletableStackPanel.DataList =
                TagToAppearListViewModelConverter.ToAppearListViewItemModel(SelectingModel.Tags, ModelsComposite.TagManager);
            _view.TagDeletableStackPanel.AddButtonStrategy = new TagStickerAddButtonStrategy(SelectingModel, ModelsComposite.TagManager);
            _view.TagDeletableStackPanel.OnModelIsAppearedChangedEvent += TagDeletableStackPanel_OnModelIsAppearedChangedEvent;
            _view.TagDeletableStackPanel.Initialize();
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
