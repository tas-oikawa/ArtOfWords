using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.ViewModels.NewFilePlus
{
    /// <summary>
    /// 展開のListViewItem
    /// </summary>
    public class StoryFrameTransferItemViewModel : NotifyPropertyChangedBase
    {
        private StoryFrameModel _source;

        /// <summary>
        /// データソース
        /// </summary>
        public StoryFrameModel Source
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
        public StoryFrameTransferItemViewModel(StoryFrameModel org)
        {
            _source = org;
        }
    }
}
