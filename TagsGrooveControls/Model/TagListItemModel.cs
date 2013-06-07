using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class TagListItemModel : INotifyPropertyChanged
    {
        private Tag _myTag;

        public string Name
        {
            get
            {
                return _myTag.Name;
            }
        }

        public int Id
        {
            get
            {
                return _myTag.Id;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { 
                if(_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public TagListItemModel(Tag tag)
        {
            _myTag = tag;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
