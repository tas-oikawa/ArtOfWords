using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Relation
{
    public class OneStoryFrameItemStoryRelationModelManager
    {
        private int _storyFrameId;
        public int StoryFrameId
        {
            get
            {
                return _storyFrameId;
            }
        }

        private Dictionary<int, ItemStoryRelationModel> _itemStoryRelModelDictionary;

        private ObservableCollection<ItemStoryRelationModel> _modelCollection;
        public ObservableCollection<ItemStoryRelationModel> ModelCollection
        {
            set
            {
                _modelCollection = value;
            }
            get
            {
                return _modelCollection;
            }
        }

        public OneStoryFrameItemStoryRelationModelManager(int storyFrameId)
        {
            _storyFrameId = storyFrameId;
            _itemStoryRelModelDictionary = new Dictionary<int, ItemStoryRelationModel>();
            _modelCollection = new ObservableCollection<ItemStoryRelationModel>();
        }

        public ItemStoryRelationModel FindModel(int id)
        {
            if (_itemStoryRelModelDictionary.ContainsKey(id))
            {
                return _itemStoryRelModelDictionary[id];
            }

            foreach (var model in _modelCollection)
            {
                if (model.ItemId == id)
                {
                    _itemStoryRelModelDictionary.Add(id, model);
                    return model;
                }
            }

            return null;
        }

        public void AddModel(ItemStoryRelationModel model)
        {
            ModelCollection.Add(model);
        }

        public void RemoveModel(ItemStoryRelationModel model)
        {
            if (_itemStoryRelModelDictionary.ContainsKey(model.ItemId))
            {
                _itemStoryRelModelDictionary.Remove(model.ItemId);
            }

            ModelCollection.Remove(model);
        }
    }
    public class ItemStoryRelationModelManager: INotifyPropertyChanged
    {
        private ObservableCollection<OneStoryFrameItemStoryRelationModelManager> _modelCollection;
        public ObservableCollection<OneStoryFrameItemStoryRelationModelManager> ModelCollection
        {
            set
            {
                _modelCollection = value;
            }
            get
            {
                return _modelCollection;
            }
        }


        private Dictionary<int, OneStoryFrameItemStoryRelationModelManager> _oneStoryItemStoryMgrDictionary;
        public ItemStoryRelationModelManager()
        {
            _oneStoryItemStoryMgrDictionary = new Dictionary<int, OneStoryFrameItemStoryRelationModelManager>();
            _modelCollection = new ObservableCollection<OneStoryFrameItemStoryRelationModelManager>();

            EventAggregator.AddIMarkableHandler += OnIMarkableAdded;
            EventAggregator.DeleteIMarkableHandler += OnIMarkableDeleted;

        }


        private void OnIMarkableAdded(object sender, AddIMarkableModelEventArgs arg)
        {
            var storyFrameModel = arg.Markable as StoryFrameModel;
            if (storyFrameModel == null)
            {
                return ;
            }

            AddStoryFrameModel(storyFrameModel.Id);
        }

        private void OnIMarkableDeleted(object sender, DeleteIMarkableModelEventArgs arg)
        {
            var storyFrameModel = arg.Markable as StoryFrameModel;
            if (storyFrameModel != null)
            {
                RemoveModel(storyFrameModel);
                return;
            }

            var itemModel = arg.Markable as ItemModel;
            if (itemModel != null)
            {
                RemoveModel(itemModel);
                return;
            }
        }

        public ItemStoryRelationModel GetNewModel(int itemId, int storyId)
        {
            var model = new ItemStoryRelationModel(itemId, storyId);

            model.Effect = ItemStoryRelationModel.EffectItems[0];

            return model;
        }

        public OneStoryFrameItemStoryRelationModelManager FindModel(int id)
        {
            if (_oneStoryItemStoryMgrDictionary.ContainsKey(id))
            {
                return _oneStoryItemStoryMgrDictionary[id];
            }

            foreach (var model in _modelCollection)
            {
                if (model.StoryFrameId == id)
                {
                    _oneStoryItemStoryMgrDictionary.Add(id, model);
                    return model;
                }
            }

            return null;
        }

        public void AddStoryFrameModel(int storyFrameModelId)
        {
            if (FindItemStoryRelationModels(storyFrameModelId) == null)
            {
                _modelCollection.Add(new OneStoryFrameItemStoryRelationModelManager(storyFrameModelId));
            }
        }

        public ObservableCollection<ItemStoryRelationModel> FindItemStoryRelationModels(int storyFrameModelId)
        {
            var model = FindModel(storyFrameModelId);

            if (model == null)
            {
                return null;
            }

            return model.ModelCollection;
        }

        public void RemoveModel(ItemStoryRelationModel model)
        {
            var modelMgr = FindModel(model.StoryFrameId);

            modelMgr.RemoveModel(model);
        }

        public void RemoveModel(StoryFrameModel model)
        {
            var modelMgr = FindModel(model.Id);

            if (_oneStoryItemStoryMgrDictionary.ContainsKey(model.Id))
            {
                _oneStoryItemStoryMgrDictionary.Remove(model.Id);
            }

            _modelCollection.Remove(modelMgr);
        }

        public void RemoveModel(ItemModel model)
        {
            foreach (var mgr in _modelCollection)
            {
                var relationModel = mgr.FindModel(model.Id);
                if (relationModel == null)
                {
                    continue;
                }

                mgr.RemoveModel(relationModel);
            }
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
