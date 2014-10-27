using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.ViewModels.NewFilePlus
{
    /// <summary>
    /// タグのListViewItem
    /// </summary>
    public class TagTransferItemViewModel : NotifyPropertyChangedBase
    {
        private SaveTagModel _source;

        /// <summary>
        /// データソース
        /// </summary>
        public SaveTagModel Source
        {
            get { return _source; }
        }


        /// <summary>
        /// 名前
        /// </summary>
        public string Name
        {
            get
            {
                return _source.Name;
            }
        }

        private bool _isSelected;

        /// <summary>
        /// 選択状態かどうか
        /// </summary>
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

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="org"></param>
        public TagTransferItemViewModel(SaveTagModel org)
        {
            _source = org;
            IsSelected = true;
        }
    }
}
