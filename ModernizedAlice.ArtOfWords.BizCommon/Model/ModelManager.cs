using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model
{
    abstract public class ModelManager
    {
        private ObservableCollection<IMarkable> _modelCollection;
        public ObservableCollection<IMarkable> ModelCollection 
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

        private Dictionary<int, IMarkable> _modelDictionary;

        public ModelManager()
        {
            _modelCollection = new ObservableCollection<IMarkable>();
            _modelDictionary = new Dictionary<int, IMarkable>();
        }

        public IMarkable FindModel(int id)
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
        
        public void AddModel(IMarkable markable)
        {
            ModelCollection.Add(markable);
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnAddIMarkable(this, new AddIMarkableModelEventArgs(markable));
        }

        public void RemoveModel(IMarkable markable)
        {
            var marks = ModelsComposite.MarkManager.GetMarks(markable);

            var RemoveMarks = marks.ToArray();

            foreach (var mark in RemoveMarks)
            {
                ModelsComposite.MarkManager.DeleteMark(mark);
            }

            _modelDictionary.Remove(markable.Id);
            ModelCollection.Remove(markable);

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
            EventAggregator.OnDeleteIMarkable(this, new DeleteIMarkableModelEventArgs(markable));
        }

        public void SwapItem(IMarkable insertItem, IMarkable targetItem)
        {
            int removedIdx = ModelCollection.IndexOf(insertItem);
            int targetIdx = ModelCollection.IndexOf(targetItem);

            if (removedIdx < targetIdx)
            {
                ModelCollection.Insert(targetIdx + 1, targetItem);
                ModelCollection.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (ModelCollection.Count + 1 > remIdx)
                {
                    ModelCollection.Insert(targetIdx, targetItem);
                    ModelCollection.RemoveAt(remIdx);
                }
            }

            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }
    }
}
