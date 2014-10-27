using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.ViewModels.NewFilePlus
{
    /// <summary>
    /// 登場人物のListViewItem
    /// </summary>
    public class CharacterTransferItemViewModel : NotifyPropertyChangedBase
    {
        private CharacterModel _source;

        /// <summary>
        /// データソース
        /// </summary>
        public CharacterModel Source
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
        public CharacterTransferItemViewModel(CharacterModel org)
        {
            _source = org;
        }
    }
}
