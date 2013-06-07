using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class Tag : INotifyPropertyChanged
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get 
            {
                return _isSelected; 
            }
            set 
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
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
