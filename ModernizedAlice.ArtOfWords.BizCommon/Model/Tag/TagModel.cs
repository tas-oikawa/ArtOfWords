using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Tag
{
    public class TagModel : INotifyPropertyChanged
    {
        public virtual bool IsBase()
        {
            return false;
        }

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

        private ObservableCollection<TagModel> _children;

        public ObservableCollection<TagModel> Children
        {
            get { return _children; }
        }


        private TagModel _parent;

        public TagModel Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }


        public TagModel(int id)
        {
            _children = new ObservableCollection<TagModel>();

            _id = id;
        }

        public bool HasInDescendent(TagModel search)
        {
            foreach (var tag in _children)
            {
                if (tag.Id == search.Id)
                {
                    return true;
                }

                if (tag.HasInDescendent(search))
                {
                    return true;
                }
            }

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
