using ModernizedAlice.ArtOfWords.BizCommon.Model.ObjectUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileSelector.ViewModels.NewFilePlus
{
    /// <summary>
    /// つよくてニューファイルのViewModel
    /// </summary>
    public class NewFilePlusViewModel : NotifyPropertyChangedBase
    {
        #region Properties
        private List<CharacterTransferItemViewModel> _characters;
        /// <summary>
        /// 登場人物のListViewItem
        /// </summary>
        public List<CharacterTransferItemViewModel> Characters 
        {
            get
            {
                return _characters;
            }
            set
            {
                _characters = value;
                OnPropertyChanged("Characters");
            }
        }

        private List<ItemTransferItemViewModel> _items;
        /// <summary>
        /// アイテムのListViewItem
        /// </summary>
        public List<ItemTransferItemViewModel> Items 
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        private List<EventTransferItemViewModel> _events;

        /// <summary>
        /// イベントのListViewItem
        /// </summary>
        public List<EventTransferItemViewModel> Events
        {
            get
            {
                return _events;
            }
            set
            {
                _events = value;
                OnPropertyChanged("Events");
            }
        }

        private bool _doTransferTags;

        /// <summary>
        /// タグを転送するかどうか
        /// </summary>
        public bool DoTransferTags
        {
            get
            {
                return _doTransferTags;
            }
            set
            {
                _doTransferTags = value;
                OnPropertyChanged("DoTransferTags");
            }
        }

        private List<StoryFrameTransferItemViewModel> _storyFrames;

        /// <summary>
        /// 展開のListViewItem
        /// </summary>
        public List<StoryFrameTransferItemViewModel> StoryFrames 
        {
            get
            {
                return _storyFrames;
            }
            set
            {
                _storyFrames = value;
                OnPropertyChanged("StoryFrames");
            }
        }

        #endregion


        #region Logic
        /// <summary>
        /// 全ての登場人物の選択状態を変更する
        /// </summary>
        /// <param name="doSelect">変更先</param>
        public void ChangeAllCharactersSelection(bool doSelect)
        {
            Characters.ForEach((e) => e.IsSelected = doSelect);
        }

        /// <summary>
        /// 全ての展開の選択状態を変更する
        /// </summary>
        /// <param name="doSelect">変更先</param>
        public void ChangeAllStoryFramesSelection(bool doSelect)
        {
            StoryFrames.ForEach((e) => e.IsSelected = doSelect);
        }

        /// <summary>
        /// 全てのアイテムの選択状態を変更する
        /// </summary>
        /// <param name="doSelect">変更先</param>
        public void ChangeAllItemsSelection(bool doSelect)
        {
            Items.ForEach((e) => e.IsSelected = doSelect);
        }

        /// <summary>
        /// 全てのイベントの選択状態を変更する
        /// </summary>
        /// <param name="doSelect">変更先</param>
        public void ChangeAllEventsSelection(bool doSelect)
        {
            Events.ForEach((e) => e.IsSelected = doSelect);
        }
        #endregion
    }
}
