using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using CommonControls;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using CommonControls.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Relation;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;

namespace StoryFrameBuildControl.Model
{
    public class StoryFrameBuildControlViewModel : INotifyPropertyChanged
    {
        private StoryFrameBuildControll _view;

        private ObservableCollection<IMarkable> _storyFrameModelCollection;
        public ObservableCollection<IMarkable> StoryFrameModelCollection
        {
            get
            {
                return _storyFrameModelCollection;
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

                    GenerateCharacterRelationPanel();
                    InitCharacterStoryListView();
                    GenerateItemRelationPanel();
                    InitItemStoryListView();
                    OnPropertyChanged("SelectingIndex");
                    OnPropertyChanged("SelectingModel");
                }
            }
            get
            {
                return _selectingIndex;
            }
        }

        private StoryFrameDetailViewModel _selectingModel;

        public StoryFrameDetailViewModel SelectingModel
        {
            get
            {
                if (SelectingIndex >= StoryFrameModelCollection.Count || SelectingIndex < 0)
                {
                    _selectingModel = null;
                    return null;
                }

                var storyFrameModel = StoryFrameModelCollection.ElementAt(SelectingIndex);
                if (_selectingModel == null || _selectingModel.Id != storyFrameModel.Id)
                {
                    _selectingModel = new StoryFrameDetailViewModel(storyFrameModel as StoryFrameModel);
                }
                return _selectingModel;
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

        public StoryFrameBuildControlViewModel()
        {
            _storyFrameModelCollection = new ObservableCollection<IMarkable>();
            _selectingIndex = -1;

            EventAggregator.AddIMarkableHandler += AddIMarkableHandler;
            EventAggregator.DeleteIMarkableHandler += DeleteIMarkableHandler;
            EventAggregator.SelectObjectForceHandler += EventAggregator_SelectObjectForceHandler;
        }

        private void AddIMarkableHandler(object sender, ModernizedAlice.ArtOfWords.BizCommon.Model.Event.AddIMarkableModelEventArgs arg)
        {
            if (arg.Markable is StoryFrameModel)
            {
                SearchWord = "";
                GetFilteredCollection();
            }
        }

        private void DeleteIMarkableHandler(object sender, ModernizedAlice.ArtOfWords.BizCommon.Model.Event.DeleteIMarkableModelEventArgs arg)
        {
            if (arg.Markable is StoryFrameModel)
            {
                int prevIndex = SelectingIndex;
                StoryFrameModelCollection.Remove(arg.Markable);
                Select(prevIndex);
            }
        }

        void EventAggregator_SelectObjectForceHandler(object sender, SelectObjectForceEventArgs e)
        {
            if (!(e.Model is StoryFrameModel))
            {
                return;
            }
            var storyFrame = e.Model as StoryFrameModel;

            Select(storyFrame);
        }


        public void GetFilteredCollection()
        {
            StoryFrameModel prevModel;
            if (SelectingModel != null)
            {
                prevModel = SelectingModel.Model;
            }
            else
            {
                prevModel = null;
            }

            int prevIndex = SelectingIndex;

            _storyFrameModelCollection.Clear();

            foreach (var model in ModelsComposite.StoryFrameModelManager.ModelCollection)
            {
                _storyFrameModelCollection.Add(model);
            }

            SearchWordUtil.DoFilterMark(SearchWord, _storyFrameModelCollection);

            if (_storyFrameModelCollection.Contains(prevModel))
            {
                Select(prevModel);
            }
            else
            {
                Select(SelectingIndex);
            }
        }

        public void Initialize(StoryFrameBuildControll view)
        {
            if (_view == null)
            {
                _view = view;

                _view.CharacterDeletableStackPanel.OnModelIsAppearedChangedEvent += OnAppearListViewItemModelChangedEvent;
                _view.ItemDeletableStackPanel.OnModelIsAppearedChangedEvent += OnAppearListViewItemModelChangedEvent;
            }

            _searchWord = "";
            _view.SearchWordBox.Text = "";
            GetFilteredCollection();

            _view.BindData(this);

            if (StoryFrameModelCollection.Count() >= 1)
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
            if (StoryFrameModelCollection.Count() > index)
            {
                selectIndex = index;
            }
            else
            {
                selectIndex = StoryFrameModelCollection.Count() - 1;
            }
            SelectingIndex = selectIndex;
        }

        public void Select(StoryFrameModel model)
        {
            int selectIndex = -1;
            if (model != null)
            {
                selectIndex = StoryFrameModelCollection.IndexOf(model);
            }

            SelectingIndex = selectIndex;
        }

        public void MoveCollection(int idx1, int idx2)
        {
            var collection = ModelsComposite.StoryFrameModelManager.ModelCollection;

            var model1 = StoryFrameModelCollection.ElementAt(idx1);
            var model2 = StoryFrameModelCollection.ElementAt(idx2);

            var moveFrom = collection.IndexOf(model1);
            var moveTo = collection.IndexOf(model2);

            collection.Move(moveFrom, moveTo);

            StoryFrameModelCollection.Move(idx1, idx2);
        }

        private void ChangeIsAppearedCharacterStoryModel(CharacterModel chara, bool isAppeared)
        {
            var charaStories = ModelsComposite.CharacterStoryModelManager.FindModel(SelectingModel.Id);
            var charaStoryModel = charaStories.FindModel(chara.Id);
            if (isAppeared)
            {
                if (charaStoryModel != null)
                {
                    return;
                }
                var model = ModelsComposite.CharacterStoryModelManager.GetNewModel(chara.Id, SelectingModel.Id);
                charaStories.AddModel(model);
            }
            else
            {
                if (charaStoryModel == null)
                {
                    return;
                }
                charaStories.RemoveModel(charaStoryModel);
            }
        }

        private void ChangeIsAppearedItemStoryModel(ItemModel item, bool isAppeared)
        {
            var itemStories = ModelsComposite.ItemStoryModelManager.FindModel(SelectingModel.Id);
            var itemStoryModel = itemStories.FindModel(item.Id);
            if (isAppeared)
            {
                if (itemStoryModel != null)
                {
                    return;
                }
                var model = ModelsComposite.ItemStoryModelManager.GetNewModel(item.Id, SelectingModel.Id);
                itemStories.AddModel(model);
            }
            else
            {
                if (itemStoryModel == null)
                {
                    return;
                }
                itemStories.RemoveModel(itemStoryModel);
            }
        }

        void OnAppearListViewItemModelChangedEvent(object sender, OnModelIsAppearedChangedEventArgs e)
        {
            if (SelectingModel == null)
            {
                return;
            }

            var lvItem = sender as AppearListViewItemModel;

            var chara = lvItem.ParentObject as CharacterModel;
            if (chara != null)
            {
                ChangeIsAppearedCharacterStoryModel(chara, e.IsAppeared);
                return;
            }
            var item = lvItem.ParentObject as ItemModel;
            if (item != null)
            {
                ChangeIsAppearedItemStoryModel(item, e.IsAppeared);
                return;
            }
        }
        
        #region アイテム領域

        private void InitItemStoryListView()
        {
            if (SelectingModel == null)
            {
                _view.ItemRelationControl.BindData(new Collection<ItemStoryRelationModel>());
            }
            else
            {
                _view.ItemRelationControl.BindData(ModelsComposite.ItemStoryModelManager.FindItemStoryRelationModels(SelectingModel.Id));
            }
        }

        private void GenerateItemRelationPanel()
        {
            if (SelectingModel == null)
            {
                _view.ItemDeletableStackPanel.DataList = new ObservableCollection<AppearListViewItemModel>();
                _view.ItemDeletableStackPanel.NoItemMessage = "ここには登録したアイテムが表示されます";
                _view.ItemDeletableStackPanel.Initialize();
                return;
            }

            var itemStories = ModelsComposite.ItemStoryModelManager.FindItemStoryRelationModels(SelectingModel.Id);

            var list = new ObservableCollection<AppearListViewItemModel>();
            foreach (var item in ModelsComposite.ItemModelManager.ModelCollection)
            {
                bool isAppeared = itemStories.Any(elem => elem.ItemId == item.Id);
                list.Add(new AppearListViewItemModel(item.Symbol, isAppeared, "登場する", "登場しない", item) { BackgroundBrush = item.ColorBrush as SolidColorBrush });
            }
            _view.ItemDeletableStackPanel.DataList = list;
            _view.ItemDeletableStackPanel.Initialize();
        }

        #endregion

        #region キャラクター領域
        private void InitCharacterStoryListView()
        {
            if (SelectingModel == null)
            {
                _view.CharaRelationControl.BindData(new Collection<CharacterStoryRelationModel>());
            }
            else
            {
                _view.CharaRelationControl.BindData(ModelsComposite.CharacterStoryModelManager.FindCharacterStoryRelationModels(SelectingModel.Id));
            }
        }

        private void GenerateCharacterRelationPanel()
        {
            if (SelectingModel == null)
            {
                return;
            }

            var charaStories = ModelsComposite.CharacterStoryModelManager.FindCharacterStoryRelationModels(SelectingModel.Id);

            var list = new ObservableCollection<AppearListViewItemModel>();
            foreach (var chara in ModelsComposite.CharacterManager.ModelCollection)
            {
                bool isAppeared = charaStories.Any(item => item.CharacterId == chara.Id);
                list.Add(new AppearListViewItemModel(chara.Symbol, isAppeared, "登場する", "登場しない", chara) { BackgroundBrush = chara.ColorBrush as SolidColorBrush });
            }

            _view.CharacterDeletableStackPanel.NoItemMessage = "ここには登録した登場人物が表示されます";
            _view.CharacterDeletableStackPanel.DataList = list;
            _view.CharacterDeletableStackPanel.Initialize();
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
