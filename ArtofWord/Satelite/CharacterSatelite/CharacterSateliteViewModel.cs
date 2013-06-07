using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.ControlUtil;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using SateliteControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.Satelite.CharacterSatelite
{
    public class CharacterSateliteViewModel : INotifyPropertyChanged
    {
        private CharacterModel _parent;

        private SateliteViewer _view;

        public CharacterSateliteViewModel(SateliteViewer viewer, CharacterModel model)
        {
            _view = viewer;
            _parent = model;

            _view.Closed += _view_Closed;
            _parent.PropertyChanged += _parent_PropertyChanged;
            _view.OnJumpEvent += _view_OnJumpEvent;
            EventAggregator.DeleteIMarkableHandler += OnIMarkableDeleted;
        }

        void _view_OnJumpEvent(object sender, object model)
        {
            EventAggregator.OnChangeTabOccured(sender, new ChangeTabEventArg(MainTabKind.CharacterTab, model));
        }


        public void Dispose()
        {
            _parent.PropertyChanged -= _parent_PropertyChanged;
            EventAggregator.DeleteIMarkableHandler -= OnIMarkableDeleted;
        }

        private void OnIMarkableDeleted(object sender, DeleteIMarkableModelEventArgs arg)
        {
            var characterModel = arg.Markable as CharacterModel;
            if (characterModel == _parent)
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

        public void NotifyChangedAll()
        {
            OnPropertyChanged("Name");
            OnPropertyChanged("FamilyName");
            OnPropertyChanged("PersonalName");
            OnPropertyChanged("MiddleName");
            OnPropertyChanged("NickName");
            OnPropertyChanged("Age");
            OnPropertyChanged("Gender");
            OnPropertyChanged("RelationWithHero");
            OnPropertyChanged("Species");
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
        public string FamilyName
        {
            get 
            {
                return _parent.LastName;
            }
        }
        public string PersonalName
        {
            get
            {
                return _parent.FirstName;
            }
        }
        public string MiddleName
        {
            get 
            {
                return _parent.MiddleName;
            }
        }
        public string NickName
        {
            get
            {
                return _parent.NickName;
            }
        }
        public string Age
        {
            get
            {
                return _parent.Age;
            }
        }
        public string Gender
        {
            get
            {
                return CharacterUtil.GetGender(_parent.Gender);
            }
        }
        public string RelationWithHero
        {
            get
            {
                return _parent.RelationWithHero;
            }
        }
        public string Species
        {
            get
            {
                return _parent.Species;
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
