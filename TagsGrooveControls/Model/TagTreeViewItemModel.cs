
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public class TagTreeViewItemModel : Tag
    {
        private bool _isBase = false;

        public override bool IsBase()
        {
            return _isBase;
        }

        public TagTreeViewItemModel(Tag tag)
            : base(tag.Id)
        {
            base.Name = tag.Name;

            if (tag.IsBase())
            {
                _isBase = true;
            }
        }

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

        private bool _isNameMode;

        public bool IsNameMode
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
    }
}
