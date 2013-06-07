using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StoryFrameBuildControl.Model
{
    public class StoryFrameDetailViewModel : INotifyPropertyChanged
    {
        private StoryFrameModel _model;

        public StoryFrameModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public StoryFrameDetailViewModel(StoryFrameModel model)
        {
            _model = model;
        }

        public int Id
        {
            get
            {
                return _model.Id;
            }
        }

        public string Name
        {
            set
            {
                if (_model.Name != value)
                {
                    _model.Name = value;
                    OnPropertyChanged("Name");
                    OnPropertyChanged("Symbol");
                }
            }
            get
            {
                return _model.Name;
            }
        }
        public string Symbol
        {
            set
            {
                if (_model.Symbol != value)
                {
                    _model.Symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
            get
            {
                return _model.Symbol;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string PlaceName
        {
            set
            {
                var place = ModelsComposite.PlaceModelManager.FindPlaceModel(_model.PlaceId);
                if (value != place.Name)
                {
                    place.Name = value;
                    OnPropertyChanged("PlaceName");
                }
            }

            get
            {
                var place = ModelsComposite.PlaceModelManager.FindPlaceModel(_model.PlaceId);
                return place.Name;
            }
        }

        public string Remarks
        {
            set
            {
                if (value != _model.Remarks)
                {
                    _model.Remarks = value;
                    OnPropertyChanged("Remark");
                }
            }
            get
            {
                return _model.Remarks;
            }
        }

        public DateTime StartDateTime
        {
            set
            {
                if (_model.StartDateTime != value)
                {
                    _model.StartDateTime = value;
                    OnPropertyChanged("StartDateTime");
                }
            }
            get
            {
                return _model.StartDateTime;
            }
        }

        public DateTime StartDate
        {
            get { return StartDateTime; }
            set
            {
                StartDateTime = new DateTime(value.Year, value.Month, value.Day, StartDateTime.Hour, StartDateTime.Minute, StartDateTime.Second);
                OnPropertyChanged("StartDate");
            }
        }


        public TimeSpan StartTime
        {
            get { return new TimeSpan(StartDateTime.Hour, StartDateTime.Minute, StartDateTime.Second); }
            set
            {
                StartDateTime = new DateTime(StartDateTime.Year, StartDateTime.Month, StartDateTime.Day, value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged("StartTime");
            }
        }


        public DateTime EndDateTime
        {
            set
            {
                if (_model.EndDateTime != value)
                {
                    _model.EndDateTime = value;
                    OnPropertyChanged("EndDateTime");
                }
            }
            get
            {
                return _model.EndDateTime;
            }
        }

        public DateTime EndDate
        {
            get { return EndDateTime; }
            set
            {
                EndDateTime = new DateTime(value.Year, value.Month, value.Day, EndDateTime.Hour, EndDateTime.Minute, EndDateTime.Second);
                OnPropertyChanged("EndDate");
            }
        }


        public TimeSpan EndTime
        {
            get { return new TimeSpan(EndDateTime.Hour, EndDateTime.Minute, EndDateTime.Second); }
            set
            {
                EndDateTime = new DateTime(EndDateTime.Year, EndDateTime.Month, EndDateTime.Day, value.Hours, value.Minutes, value.Seconds);
                OnPropertyChanged("EndTime");
            }
        }

        public Brush ColorBrush
        {
            set
            {
                if (_model.ColorBrush != value)
                {
                    _model.ColorBrush = value;
                    OnPropertyChanged("ColorBrush");
                    OnPropertyChanged("MarkBrush");
                }
            }
            get
            {
                return _model.ColorBrush;
            }
        }

        public Brush MarkBrush
        {
            get
            {
                return _model.MarkBrush;
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
