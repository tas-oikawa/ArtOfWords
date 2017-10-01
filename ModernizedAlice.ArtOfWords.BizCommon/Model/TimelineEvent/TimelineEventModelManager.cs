using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent
{
    public class TimelineEventModelManager : INotifyPropertyChanged
    {        
        private ObservableCollection<TimelineEventModel> _modelCollection;
        public ObservableCollection<TimelineEventModel> ModelCollection
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

        private Dictionary<int, TimelineEventModel> _modelDictionary;

        public TimelineEventModelManager()
        {
            _modelCollection = new ObservableCollection<TimelineEventModel>();
            _modelDictionary = new Dictionary<int, TimelineEventModel>();
        }

        public TimelineEventModel FindModel(int id)
        {
            if (_modelDictionary.ContainsKey(id))
            {
                return _modelDictionary[id];
            }

            foreach (var model in _modelCollection)
            {
                if (model.Id == id)
                {
                    _modelDictionary.Add(id, model);
                    return model;
                }
            }

            return null;
        }

        protected int GetUniqueId()
        {
            int maxId = 0;

            foreach (var model in _modelCollection)
            {
                if (maxId < model.Id)
                {
                    maxId = model.Id;
                }
            }

            return (maxId + 1);
        }

        public TimelineEventModel GetNewModel()
        {
            var model = new TimelineEventModel();
            model.Title = "新しいイベント";
            model.Id = GetUniqueId();
            model.Participants = new List<int>();
            model.StartDateTime = DateTime.Today;
            model.EndDateTime = DateTime.Today.AddHours(1);
            model.Detail = "";

            return model;
        }

        public void AddModel(TimelineEventModel model)
        {
            ModelCollection.Add(model);
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }

        public void RemoveModel(TimelineEventModel model)
        {
            _modelDictionary.Remove(model.Id);
            ModelCollection.Remove(model);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }

        public void RemoveParticipants(CharacterModel chara)
        {
            List<TimelineEventModel> removeModels = new List<TimelineEventModel>();

            foreach (var model in ModelCollection)
            {
                if (model.Participants.Contains(chara.Id))
                {
                    model.Participants.Remove(chara.Id);
                }

                if (model.Participants.Count() == 0)
                {
                    removeModels.Add(model);
                }
            }

            foreach (var item in removeModels)
            {
                ModelCollection.Remove(item);
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
