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
