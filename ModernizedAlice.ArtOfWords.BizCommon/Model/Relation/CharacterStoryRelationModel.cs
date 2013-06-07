using ModernizedAlice.ArtOfWords.BizCommon.Event;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Relation
{
    public class CharacterStoryRelationModel : INotifyPropertyChanged
    {
        private int _characterId;
        public int CharacterId
        {
            set
            {
                if (_characterId != value)
                {
                    _characterId = value;
                    OnPropertyChanged("CharacterId");
                }
            }
            get
            {
                return _characterId;
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
        public static List<String> PositionItems
        {
            get
            {
                return _positionItems;
            }
        }

        private static List<String> _positionItems = new List<String>()
        {
            "未設定",
            "話をリードする",
            "リーダーをサポートする",
            "話をかき回す",
            "話を観察する",
            "話をわかりやすくする",
            "話の裏で暗躍する",
            "賑やかし（ガヤ）",
            "テーマとして上がる",
        };

        private string _position;
        public string Position
        {
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged("Position");
                }
            }
            get
            {
                return _position;
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
                var chara = ModelsComposite.CharacterManager.FindCharacter(_characterId);
                return chara.Name;
            }
        }

        public CharacterStoryRelationModel()
        {
        }

        public CharacterStoryRelationModel(int charaId, int storyId)
        {
            _characterId = charaId;
            _storyFrameId = storyId;


            var chara = ModelsComposite.CharacterManager.FindCharacter(_characterId);
            chara.PropertyChanged += chara_PropertyChanged;
        }

        public void DoActionAfterLoad()
        {
            var chara = ModelsComposite.CharacterManager.FindCharacter(_characterId);
            chara.PropertyChanged += chara_PropertyChanged;
        }

        void chara_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
