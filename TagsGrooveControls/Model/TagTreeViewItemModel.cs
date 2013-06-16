using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class TagTreeViewItemModel : TagModel
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

                    if (!value)
                    {
                        IsNameMode = false;
                    }

                    OnPropertyChanged("IsSelected");
                }
            }
        }

        private bool _isExpanded = true;

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

        private bool _isNameMode;

        public virtual bool IsNameMode
        {
            get
            {
                return _isNameMode;
            }
            set
            {
                if (_isNameMode != value)
                {
                    _isNameMode = value;
                    OnPropertyChanged("IsNameMode");
                }
            }
        }

        public TagTreeViewItemModel(int orgId)
            : base(orgId)
        {
        }

        public override bool IsBase()
        {
            return false;
        }
    }
}
