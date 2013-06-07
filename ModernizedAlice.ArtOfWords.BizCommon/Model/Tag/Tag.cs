using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Tag
{
    public class Tag : INotifyPropertyChanged
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set 
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private ObservableCollection<Tag> _children;

        public ObservableCollection<Tag> Children
        {
            get { return _children; }
        }


        private Tag _parent;

        public Tag Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }


        public Tag(int id)
        {
            _children = new ObservableCollection<Tag>();

            _id = id;
        }

        public bool IsChild(Tag tag)
        {
            foreach (var child in _children)
            {
                if (child.Id == tag.Id)
                {
                    return true;
                }
                if (child.IsChild(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool IsBase()
        {
            return false;
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
