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

    public class TagTreeViewItemModelManager
    {
        /// <summary>
        /// タグの一覧。ID:0は特別な番号
        /// </summary>
        private Dictionary<int, TagTreeViewItemModel> _tagDictionary;

        private List<TagTreeViewItemModel> _deletedItemList;

        public TagTreeViewItemModelManager()
        {
            _tagDictionary = new Dictionary<int, TagTreeViewItemModel>();
            _deletedItemList = new List<TagTreeViewItemModel>();
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

        public void ConnectTags(TagTreeViewItemModel parent, TagTreeViewItemModel child)
        {
            parent.Children.Add(child);
            child.Parent = parent;
        }

        public void Add(TagTreeViewItemModel tag)
        {
            _tagDictionary.Add(tag.Id, tag);
        }

        private void RemoveFromDictonary(TagTreeViewItemModel tag)
        {
            _tagDictionary.Remove(tag.Id);
        }

        public void RemoveChildren(TagTreeViewItemModel tag)
        {
            foreach(var child in tag.Children)
            {
                RemoveFromDictonary(tag);
                RemoveChildren(child);
                OnTagRemoved(tag);
            }

            tag.Children.Clear();
        }

        public void ConnectChildrenToParent(TagTreeViewItemModel tag)
        {
            foreach (var child in tag.Children)
            {
                ConnectTags(tag, child);
            }
        }

        public void DisconnectFromParent(TagTreeViewItemModel tag)
        {
            tag.Parent.Children.Remove(tag);
            tag.Parent = null;
        }

        public void Remove(TagTreeViewItemModel tag, RemoveType type)
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


        public TagTreeViewItemModel GenerateNewTag()
        {
            return new TagTreeViewItemModel(GetNewId()) { Name = "名前の無いタグ" };
        }

        public TagTreeViewItemModel GetBaseTag()
        {
            return _tagDictionary.First((e) => (e.Value is BaseTag)).Value;
        }

        public delegate void TagRemovedEventHandler(object sender, TagTreeViewItemModel tag);
        public event TagRemovedEventHandler TagRemoved;

        public void OnTagRemoved(TagTreeViewItemModel tag)
        {
            if (TagRemoved != null)
            {
                TagRemoved(this, tag);
            }
        }
    }
}
