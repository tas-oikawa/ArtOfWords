using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Relation
{
    public class ItemStoryRelationModel : INotifyPropertyChanged
    {
        private int _itemId;
        public int ItemId
        {
            set
            {
                if (_itemId != value)
                {
                    _itemId = value;
                    OnPropertyChanged("ItemId");
                }
            }
            get
            {
                return _itemId;
            }
        }

        private int _storyFrameId;
        public int StoryFrameId
        {
            set
            {
                if (_storyFrameId != value)
                {
                    _storyFrameId = value;
                    OnPropertyChanged("StoryFrameId");
                }
            }
            get
            {
                return _storyFrameId;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public static List<String> EffectItems
        {
            get
            {
                return _effectItems;
            }
        }

        private static List<String> _effectItems = new List<String>()
        {
            "未設定",
            "話の舞台になる",
            "話を引っ張る",
            "話を展開させる",
            "人の心を映す",
            "人の心を変化させる",
            "伏線として登場する",
            "読者を惹きつける",
            "読者をミスリードさせる",
            "存在するだけ",
            "エピソードとして上がる",
        };

        private string _effect;
        public string Effect
        {
            set
            {
                if (_effect != value)
                {
                    _effect = value;
                    OnPropertyChanged("Effect");
                }
            }
            get
            {
                return _effect;
            }
        }

        private string _behavior;
        public string Behavior
        {
            set
            {
                if (_behavior != value)
                {
                    _behavior = value;
                    OnPropertyChanged("Behavior");
                }
            }
            get
            {
                return _behavior;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public string Name
        {
            get
            {
                var item = ModelsComposite.ItemModelManager.FindItemModel(_itemId);
                return item.Name;
            }
        }

        public ItemStoryRelationModel()
        {
        }

        public ItemStoryRelationModel(int itemId, int storyId)
        {
            _itemId = itemId;
            _storyFrameId = storyId;


            var item = ModelsComposite.ItemModelManager.FindItemModel(itemId);
            item.PropertyChanged += item_PropertyChanged;

        }

        public void DoActionAfterLoad()
        {
            var item = ModelsComposite.ItemModelManager.FindItemModel(_itemId);
            item.PropertyChanged += item_PropertyChanged;
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                OnPropertyChanged("Name");
            }
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
            EventAggregator.OnModelDataChanged(this, new ModelValueChangedEventArgs());
        }

        #endregion
    }
}
