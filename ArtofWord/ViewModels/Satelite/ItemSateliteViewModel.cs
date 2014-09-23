using ModernizedAlice.ArtOfWords.BizCommon.ControlUtil;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using SateliteControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.ViewModels.Satelite.ItemSatelite
{
    public class ItemSateliteViewModel : INotifyPropertyChanged
    {
        private ItemModel _parent;

        private SateliteViewer _view;

        public ItemSateliteViewModel(SateliteViewer viewer, ItemModel model)
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
            EventAggregator.OnChangeTabOccured(sender, new ChangeTabEventArg(MainTabKind.ItemTab, e.Model));
        }

        public void Dispose()
        {
            _parent.PropertyChanged -= _parent_PropertyChanged;
            EventAggregator.DeleteIMarkableHandler -= OnIMarkableDeleted;
        }

        private void OnIMarkableDeleted(object sender, DeleteIMarkableModelEventArgs arg)
        {
            var itemModel = arg.Markable as ItemModel;
            if (itemModel == _parent)
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
            OnPropertyChanged("Kind");
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

        public string Kind
        {
            get
            {
                return ItemUtil.GetItem(_parent.Kind);
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
