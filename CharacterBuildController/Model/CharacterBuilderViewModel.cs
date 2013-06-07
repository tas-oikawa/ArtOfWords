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

namespace CharacterBuildControll.Model
{
    public class CharacterBuildViewModel : INotifyPropertyChanged
    {
        private CharacterBuildControll _view;

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
                    OnPropertyChanged("SelectingIndex");

                    OnPropertyChanged("SelectingModel");
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

        public void Initialize(CharacterBuildControll view)
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
