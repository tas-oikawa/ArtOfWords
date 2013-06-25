using ModernizedAlice.ArtOfWords.BizCommon.Event;
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
        private Dictionary<int, TagModel> _tagDictionary;

        public Dictionary<int, TagModel> TagDictionary
        {
            get { return _tagDictionary; }
            set { _tagDictionary = value; }
        }

        public TagManager()
        {
            _tagDictionary = new Dictionary<int, TagModel>();
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

        public void ConnectTags(TagModel parent, TagModel child)
        {
            parent.Children.Add(child);
            child.Parent = parent;
        }

        public void Add(TagModel tag)
        {
            _tagDictionary.Add(tag.Id, tag);
            DoPostAdd(tag);
        }

        protected virtual void DoPostAdd(TagModel tag)
        {
            EventAggregator.OnTagModelModified(this, new Event.TagModelModifiedEventArgs()
            {
                Kind = Event.TagModelModifiedKind.Add,
                ModifiedTag = tag,
            }
            );
        }

        protected virtual void RemoveFromDictonary(TagModel tag)
        {
            _tagDictionary.Remove(tag.Id);
        }

        public void RemoveChildren(TagModel tag)
        {
            foreach(var child in tag.Children)
            {
                RemoveFromDictonary(child);
                RemoveChildren(child);
                OnTagRemoved(child);

                DoPostRemove(child);
            }

            tag.Children.Clear();
        }

        public void DisconnectFromParent(TagModel tag)
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

        protected virtual void DoPostRemove(TagModel tag)
        {
            EventAggregator.OnTagModelModified(this, new Event.TagModelModifiedEventArgs()
            {
                Kind = Event.TagModelModifiedKind.Deleted,
                ModifiedTag = tag,
            }
            );
        }

        public virtual void Remove(TagModel tag)
        {
            if (tag.Parent == null)
            {
                return;
            }

            RemoveChildren(tag);


            RemoveFromDictonary(tag);
            OnTagRemoved(tag);
            DisconnectFromParent(tag);

            DoPostRemove(tag);
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

        public virtual TagModel GenerateNewTag()
        {
            return new TagModel(GetNewId()){Name = "名前の無いタグ"};
        }

        public TagModel GetBaseTag()
        {
            return _tagDictionary.First((e) => (e.Value.IsBase())).Value;
        }

        public delegate void TagRemovedEventHandler(object sender, TagModel tag);
        public event TagRemovedEventHandler TagRemoved;

        public void OnTagRemoved(TagModel tag)
        {
            if (TagRemoved != null)
            {
                TagRemoved(this, tag);
            }
        }
    }
}
