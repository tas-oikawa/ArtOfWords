using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimelineControl.Model
{
    public class EventModelManager
    {
        private List<EventModel> _eventModelCollection;

        public EventModelManager()
        {
            _eventModelCollection = new List<EventModel>();
        }

        public void Add(EventModel eventModel)
        {
            _eventModelCollection.Add(eventModel);
        }
        public void Remove(EventModel eventModel)
        {
            _eventModelCollection.Remove(eventModel);
        }

        public List<EventModel> GetEventModel(int id)
        {
            var sortedCollection =  from evt in _eventModelCollection
                                    orderby evt.StartDateTime, evt.EndDateTime
                                    select evt;

            List<EventModel> _eventModelList = new List<EventModel>();

            foreach (var evt in sortedCollection)
            {
                if (evt.Participants.Contains(id))
                {
                    _eventModelList.Add(evt);
                }
            }

            return _eventModelList;
        }

        public Dictionary<int, List<EventModel>> GetEventModel(DateTime startTime, DateTime endTime)
        {
            var sortedCollection =  from evt in _eventModelCollection
                                    orderby evt.StartDateTime, evt.EndDateTime
                                    select evt;

            Dictionary<int, List<EventModel>> eventModelDictionary = new Dictionary<int, List<EventModel>>();

            foreach (var evt in sortedCollection)
            {                
                if (endTime < evt.StartDateTime)
                {
                    continue;
                }
                if (startTime > evt.EndDateTime)
                {
                    continue;
                }

                foreach (var id in evt.Participants)
                {
                    if (!eventModelDictionary.ContainsKey(id))
                    {
                        eventModelDictionary.Add(id, new List<EventModel>());
                    }

                    eventModelDictionary[id].Add(evt);
                }
            }

            return eventModelDictionary;
        }
    }
}
