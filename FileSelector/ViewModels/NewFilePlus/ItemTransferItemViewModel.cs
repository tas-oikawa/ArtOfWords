using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.ViewModels.NewFilePlus
{
    /// <summary>
    /// アイテムのListViewItem
    /// </summary>
    public class ItemTransferItemViewModel : NotifyPropertyChangedBase
    {
        private ItemModel _source;

        /// <summary>
        /// データソース
        /// </summary>
        public ItemModel Source
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
        public ItemTransferItemViewModel(ItemModel org)
        {
            _source = org;
        }
    }
}
