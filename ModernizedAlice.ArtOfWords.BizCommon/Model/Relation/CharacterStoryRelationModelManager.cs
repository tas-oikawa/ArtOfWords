using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Relation
{
    public class OneStoryFrameCharacterStoryRelationModelManager
    {
        private int _storyFrameId;
        public int StoryFrameId
        {
            get
            {
                return _storyFrameId;
            }
        }

        private Dictionary<int, CharacterStoryRelationModel> _characterStoryRelModelDictionary;

        private ObservableCollection<CharacterStoryRelationModel> _modelCollection;
        public ObservableCollection<CharacterStoryRelationModel> ModelCollection
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

        public OneStoryFrameCharacterStoryRelationModelManager(int storyFrameId)
        {
            _storyFrameId = storyFrameId;
            _characterStoryRelModelDictionary = new Dictionary<int, CharacterStoryRelationModel>();
            _modelCollection = new ObservableCollection<CharacterStoryRelationModel>();
        }

        public CharacterStoryRelationModel FindModel(int id)
        {
            if (_characterStoryRelModelDictionary.ContainsKey(id))
            {
                return _characterStoryRelModelDictionary[id];
            }

            foreach (var model in _modelCollection)
            {
                if (model.CharacterId == id)
                {
                    _characterStoryRelModelDictionary.Add(id, model);
                    return model;
                }
            }

            return null;
        }

        public void AddModel(CharacterStoryRelationModel model)
        {
            ModelCollection.Add(model);
        }

        public void RemoveModel(CharacterStoryRelationModel model)
        {
            if (_characterStoryRelModelDictionary.ContainsKey(model.CharacterId))
            {
                _characterStoryRelModelDictionary.Remove(model.CharacterId);
            }

            ModelCollection.Remove(model);
        }
    }
    public class CharacterStoryRelationModelManager: INotifyPropertyChanged
    {
        private ObservableCollection<OneStoryFrameCharacterStoryRelationModelManager> _modelCollection;
        public ObservableCollection<OneStoryFrameCharacterStoryRelationModelManager> ModelCollection
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


        private Dictionary<int, OneStoryFrameCharacterStoryRelationModelManager> _oneStoryCharaStoryMgrDictionary;
        public CharacterStoryRelationModelManager()
        {
            _oneStoryCharaStoryMgrDictionary = new Dictionary<int, OneStoryFrameCharacterStoryRelationModelManager>();
            _modelCollection = new ObservableCollection<OneStoryFrameCharacterStoryRelationModelManager>();

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

            var characterModel = arg.Markable as CharacterModel;
            if (characterModel != null)
            {
                RemoveModel(characterModel);
                return;
            }
        }

        public CharacterStoryRelationModel GetNewModel(int characterId, int storyId)
        {
            var model = new CharacterStoryRelationModel(characterId, storyId);

            model.Position = CharacterStoryRelationModel.PositionItems[0];

            return model;
        }

        public OneStoryFrameCharacterStoryRelationModelManager FindModel(int id)
        {
            if (_oneStoryCharaStoryMgrDictionary.ContainsKey(id))
            {
                return _oneStoryCharaStoryMgrDictionary[id];
            }

            foreach (var model in _modelCollection)
            {
                if (model.StoryFrameId == id)
                {
                    _oneStoryCharaStoryMgrDictionary.Add(id, model);
                    return model;
                }
            }

            return null;
        }

        public void AddStoryFrameModel(int storyFrameModelId)
        {
            if (FindCharacterStoryRelationModels(storyFrameModelId) == null)
            {
                _modelCollection.Add(new OneStoryFrameCharacterStoryRelationModelManager(storyFrameModelId));
            }
        }

        public ObservableCollection<CharacterStoryRelationModel> FindCharacterStoryRelationModels(int storyFrameModelId)
        {
            var model = FindModel(storyFrameModelId);

            if (model == null)
            {
                return null;
            }

            return model.ModelCollection;
        }

        public void RemoveModel(CharacterStoryRelationModel model)
        {
            var modelMgr = FindModel(model.StoryFrameId);

            modelMgr.RemoveModel(model);
        }

        public void RemoveModel(StoryFrameModel model)
        {
            var modelMgr = FindModel(model.Id);

            if (_oneStoryCharaStoryMgrDictionary.ContainsKey(model.Id))
            {
                _oneStoryCharaStoryMgrDictionary.Remove(model.Id);
            }

            _modelCollection.Remove(modelMgr);
        }

        public void RemoveModel(CharacterModel model)
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
