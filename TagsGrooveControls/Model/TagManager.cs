using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TagsGrooveControls.Model
{
    public enum RemoveType
    {
        RemoveRelatedChildren,
        ConnectRelatedChildrenToParent,
    }

    public class TagManager
    {
        /// <summary>
        /// タグの一覧。ID:0は特別な番号
        /// </summary>
        private Dictionary<int, Tag> _tagDictionary;

        public TagManager()
        {
            _tagDictionary = new Dictionary<int, Tag>();
        }

        private int GetNewId()
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

        private void RemoveFromDictonary(Tag tag)
        {
            _tagDictionary.Remove(tag.Id);
        }

        public void RemoveChildren(Tag tag)
        {
            foreach(var child in tag.Children)
            {
                RemoveFromDictonary(tag);
                RemoveChildren(child);
                OnTagRemoved(tag);
            }

            tag.Children.Clear();
        }

        public void ConnectChildrenToParent(Tag tag)
        {
            foreach (var child in tag.Children)
            {
                ConnectTags(tag, child);
            }
        }

        public void DisconnectFromParent(Tag tag)
        {
            tag.Parent.Children.Remove(tag);
            tag.Parent = null;
        }

        public void Remove(Tag tag, RemoveType type)
        {
            if (tag.Parent == null)
            {
                return;
            }

            if (type == RemoveType.ConnectRelatedChildrenToParent)
            {
                ConnectChildrenToParent(tag);
            }
            else
            {
                RemoveChildren(tag);
            }

            RemoveFromDictonary(tag);
            OnTagRemoved(tag);
            DisconnectFromParent(tag);
        }


        public Tag GenerateNewTag()
        {
            return new Tag(GetNewId()){Name = "名前の無いタグ"};
        }

        public Tag GetBaseTag()
        {
            return _tagDictionary.First((e) => (e.Value is BaseTag)).Value;
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
