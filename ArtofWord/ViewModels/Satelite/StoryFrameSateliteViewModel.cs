using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.ControlUtil;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using SateliteControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.ViewModels.Satelite.StoryFrameSatelite
{
    public class StoryFrameSateliteViewModel : INotifyPropertyChanged
    {
        private StoryFrameModel _parent;

        private SateliteViewer _view;


        public StoryFrameSateliteViewModel(SateliteViewer viewer, StoryFrameModel model)
        {
            _view = viewer;
            _parent = model;

            _view.Closed += _view_Closed;
            _parent.PropertyChanged += _parent_PropertyChanged;
            _view.OnJumpEvent += _view_OnJumpEvent;
            EventAggregator.DeleteIMarkableHandler += OnIMarkableDeleted;
        }

        private void _view_OnJumpEvent(object sender, OnJumpOccuredEventArgs e)
        {
            EventAggregator.OnChangeTabOccured(sender, new ChangeTabEventArg(MainTabKind.StoryFrameTab, e.Model));
        }

        public void Dispose()
        {
            _parent.PropertyChanged -= _parent_PropertyChanged;
            EventAggregator.DeleteIMarkableHandler -= OnIMarkableDeleted;
        }

        private void OnIMarkableDeleted(object sender, DeleteIMarkableModelEventArgs arg)
        {
            var storyFrameModel = arg.Markable as StoryFrameModel;
            if (storyFrameModel == _parent)
            {
                _view.Close();
            }
        }

        void _parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyChangedAll();
        }

        void _view_Closed(object sender, EventArgs e)
        {
            Dispose();
        }

        private void NotifyChangedAll()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("Place");
            OnPropertyChanged("StartDateTime");
            OnPropertyChanged("EndDateTime");
            OnPropertyChanged("CharacterList");
            OnPropertyChanged("ItemList");
            OnPropertyChanged("Remarks");
        }

        #region Properties
        public string Name
        {
            get
            {
                return _parent.Name;
            }
        }

        public string Place
        {
            get
            {
                var place = ModelsComposite.PlaceModelManager.FindPlaceModel(_parent.PlaceId);
                if(place != null)
                {
                    return place.Name;
                }
                return "";
            }
        }

        public string StartDateTime
        {
            get 
            {
                return _parent.StartDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        public string EndDateTime
        {
            get 
            {
                return _parent.EndDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        public List<CharacterModel> CharacterList
        {
            get
            {
                var list = ModelsComposite.CharacterStoryModelManager.FindCharacterStoryRelationModels(_parent.Id);

                List<CharacterModel> models = new List<CharacterModel>();
                foreach (var data in list)
                {
                    var chara = ModelsComposite.CharacterManager.FindCharacter(data.CharacterId);
                    if (chara != null)
                    {
                        models.Add(chara);
                    }
                }

                return models;
            }
        }

        public List<ItemModel> ItemList
        {
            get
            {
                var list = ModelsComposite.ItemStoryModelManager.FindItemStoryRelationModels(_parent.Id);
                List<ItemModel> models = new List<ItemModel>();

                foreach (var data in list)
                {
                    var item = ModelsComposite.ItemModelManager.FindItemModel(data.ItemId);
                    if (item != null)
                    {
                        models.Add(item);
                    }
                }

                return models;
            }
        }

        public string Remarks
        {
            get
            {
                return _parent.Remarks;
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
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }

        #endregion

    }
}
