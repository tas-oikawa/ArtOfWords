using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model
{
    [Serializable()]
    public class NovelSettingModel : INotifyPropertyChanged
    {
        private int _charactersPerLineCount = 40;

        public int CharactersPerLineCount
        {
            get { return _charactersPerLineCount; }
            set 
            {
                if (_charactersPerLineCount != value)
                {
                    _charactersPerLineCount = value;
                    OnPropertyChanged("CharactersPerLineCount");
                };
            }
        }

        private int _lineCountPerPage = 30;

        public int LineCountPerPage
        {
            get { return _lineCountPerPage; }
            set
            {
                if (_lineCountPerPage != value)
                {
                    _lineCountPerPage = value;
                    OnPropertyChanged("LineCountPerPage");
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }
    }
}
