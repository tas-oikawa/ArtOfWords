using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TagsGrooveControls.View;

namespace TagsGrooveControls.Model
{
    public class TagSelectorViewModel : INotifyPropertyChanged
    {
        private TagsGrooveTreeViewModel _treeViewModel;
        private TagsGrooveTreeView _treeView;

        private ObservableCollection<TagModel> _selectingTags;

        public ObservableCollection<TagModel> SelectingTags
        {
            get { return _selectingTags; }
            set { _selectingTags = value; }
        }

        public bool IsSelectable
        {
            get
            {
                if (IsNoTagsSelecting())
                {
                    return false;
                }

                if (IsBaseTagSelecting())
                {
                    return false;
                }

                if (IsAlreadySelecting())
                {
                    return false;
                }

                return true;
            }
        }

        public string SelectButtonToolTip
        {
            get
            {
                if (IsNoTagsSelecting())
                {
                    return "ツリーからタグを選択してください。";
                }

                if (IsBaseTagSelecting())
                {
                    return "一番上のタグは貼れないことになってます。他のを選んでくださいね。";
                }

                if (IsAlreadySelecting())
                {
                    return "このタグはすでに貼られています。他のを選びましょう。";
                }

                return "現在選択中のタグを貼り付けます。";
            }
        }

        public TagSelectorViewModel()
        {
            _selectingTags = new ObservableCollection<TagModel>();
        }

        public void SetView(TagsGrooveTreeView treeView)
        {
            _treeView = treeView;

            _treeViewModel = new TagsGrooveTreeViewModel(_treeView);

            _treeViewModel.Init(ModelsComposite.TagManager);
            InitSelectingTag();

            _treeView.TagTreeView.SelectedItemChanged += TagTreeView_SelectedItemChanged;
            _treeViewModel.TagRemoved += _treeViewModel_TagRemoved;
        }

        public void InitSelectingTag()
        {
            var tagIdList = new List<int>();

            foreach (var selectedTag in _selectingTags)
            {
                tagIdList.Add(selectedTag.Id);
            }
            _selectingTags.Clear();

            foreach (var selectId in tagIdList)
            {
                SelectingTags.Add(_treeViewModel.Manager.TagDictionary[selectId]);
            }
        }

        public void UpdateModelsComposite()
        {
            TagManagerReverseConverter.Reflect(ModelsComposite.TagManager, _treeViewModel.Manager);
        }

        void _treeViewModel_TagRemoved(object sender, TagModel deleteTag)
        {
            if (_selectingTags.Contains(deleteTag))
            {
                _selectingTags.Remove(deleteTag);
            }
        }

        private void OnSelectingButtonChanged()
        {
            OnPropertyChanged("IsSelectable");
            OnPropertyChanged("SelectButtonToolTip");
        }

        void TagTreeView_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            OnSelectingButtonChanged();
        }

        public void AddSelection()
        {
            var selectedTag = _treeViewModel.GetSelectingTag();

            if (selectedTag == null)
            {
                return;
            }

            if (selectedTag.IsBase())
            {
                return;
            }

            if (_selectingTags.Contains(selectedTag))
            {
                return;
            }

            SelectingTags.Add(selectedTag);

            OnSelectingButtonChanged();
        }

        public void RemoveSelection(TagModel selectingTag)
        {
            if (selectingTag == null)
            {
                return;
            }

            if (!_selectingTags.Contains(selectingTag))
            {
                return;
            }

            SelectingTags.Remove(selectingTag);

            OnSelectingButtonChanged();
        }

        private bool IsNoTagsSelecting()
        {
            if (_treeViewModel == null)
            {
                return true;
            }

            return (_treeViewModel.GetSelectingTag() == null);
        }

        private bool IsBaseTagSelecting()
        {
            if (_treeViewModel == null)
            {
                return false;
            }

            return (_treeViewModel.GetSelectingTag() is BaseTagTreeViewItemModel);
        }
        private bool IsAlreadySelecting()
        {
            if (_treeViewModel == null)
            {
                return false;
            }

            return _selectingTags.Any((e) => (e.Id ==  _treeViewModel.GetSelectingTag().Id));
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
