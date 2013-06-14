using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.Tag
{
    public class TagManager
    {
        /// <summary>
        /// タグの一覧。ID:0は特別な番号
        /// </summary>
        private Dictionary<int, Tag> _tagDictionary;

        public Dictionary<int, Tag> TagDictionary
        {
            get { return _tagDictionary; }
            set { _tagDictionary = value; }
        }

        public TagManager()
        {
            _tagDictionary = new Dictionary<int, Tag>();
        }

        protected virtual int GetNewId()
        {
            int _maxId = 0;
            foreach (var tag in _tagDictionary.Values)
            {
                if (_maxId < tag.Id)
                {
                    _maxId = tag.Id;
                }
            }

            return _maxId + 1;
        }

        public void ConnectTags(Tag parent, Tag child)
        {
            parent.Children.Add(child);
            child.Parent = parent;
        }

        public void Add(Tag tag)
        {
            _tagDictionary.Add(tag.Id, tag);
        }

        protected virtual void RemoveFromDictonary(Tag tag)
        {
            _tagDictionary.Remove(tag.Id);
        }

        public void RemoveChildren(Tag tag)
        {
            foreach(var child in tag.Children)
            {
                RemoveFromDictonary(child);
                RemoveChildren(child);
                OnTagRemoved(child);
            }

            tag.Children.Clear();
        }

        public void DisconnectFromParent(Tag tag)
        {
            tag.Parent.Children.Remove(tag);
            tag.Parent = null;
        }

        public virtual void Remove(int tagId)
        {
            if (_tagDictionary.ContainsKey(tagId))
            {
                Remove(_tagDictionary[tagId]);
            }
        }

        public virtual void Remove(Tag tag)
        {
            if (tag.Parent == null)
            {
                return;
            }

            RemoveChildren(tag);


            RemoveFromDictonary(tag);
            OnTagRemoved(tag);
            DisconnectFromParent(tag);
        }

        public void ReconnectAllToBaseTag()
        {
            foreach (var tag in _tagDictionary.Values)
            {
                if (tag.IsBase())
                {
                    continue;
                }

                DisconnectFromParent(tag);
                ConnectTags(GetBaseTag(), tag);
            }
        }

        public virtual Tag GenerateNewTag()
        {
            return new Tag(GetNewId()){Name = "名前の無いタグ"};
        }

        public Tag GetBaseTag()
        {
            return _tagDictionary.First((e) => (e.Value.IsBase())).Value;
        }

        public delegate void TagRemovedEventHandler(object sender, Tag tag);
        public event TagRemovedEventHandler TagRemoved;

        public void OnTagRemoved(Tag tag)
        {
            if (TagRemoved != null)
            {
                TagRemoved(this, tag);
            }
        }
    }
}
