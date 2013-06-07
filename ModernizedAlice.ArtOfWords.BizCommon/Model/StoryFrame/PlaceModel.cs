using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame
{
    public class PlaceModel : IMarkable, INotifyPropertyChanged
    {
        private int _id;
        public int Id
        {
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }

            get
            {
                return _id;
            }
        }

        private string _name;
        public string Name
        {
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
            get
            {
                return _name;
            }
        }

        private Brush _colorBrush { set; get; }
        public Brush ColorBrush
        {
            set
            {
                if (_colorBrush != value)
                {
                    _colorBrush = value;
                    OnPropertyChanged("ColorBrush");
                    OnPropertyChanged("MarkBrush");
                }
            }
            get
            {
                return _colorBrush;
            }
        }


        private String _symbol;
        public String Symbol
        {
            set
            {
                if (_symbol != value)
                {
                    _symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
            get
            {
                return _symbol;
            }
        }
        /// <summary>
        /// このモデルが現在も有効かどうかを返す。有効でない場合、このモデルを使ってはいけない
        /// </summary>
        public bool IsValid { set; get; }

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
