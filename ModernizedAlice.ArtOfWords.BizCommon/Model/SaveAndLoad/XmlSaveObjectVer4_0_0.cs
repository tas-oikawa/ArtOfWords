using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Relation;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad
{
    [Serializable()]
    [XmlRoot("XmlSaveObject")]
    [System.Xml.Serialization.XmlInclude(typeof(CharacterMark))]
    [System.Xml.Serialization.XmlInclude(typeof(StoryFrameMark))]
    [System.Xml.Serialization.XmlInclude(typeof(ItemModel))]
    [System.Xml.Serialization.XmlInclude(typeof(CharacterModel))]
    [System.Xml.Serialization.XmlInclude(typeof(StoryFrameModel))]
    [System.Xml.Serialization.XmlInclude(typeof(PlaceModel))]
    [System.Xml.Serialization.XmlInclude(typeof(DocumentModel))]
    [System.Xml.Serialization.XmlInclude(typeof(SaveTagModel))]
    [System.Xml.Serialization.XmlInclude(typeof(NovelSettingModel))]
    [System.Xml.Serialization.XmlInclude(typeof(SolidColorBrush))]
    [System.Xml.Serialization.XmlInclude(typeof(MatrixTransform))]
    public class XmlSaveObjectVer4_0_0
    {
        public Collection<CharacterModel> CharacterModelCollection;
        public Collection<PlaceModel> PlaceModelCollection;
        public Collection<StoryFrameModel> StoryFrameModelCollection;
        public Collection<ItemModel> ItemModelCollection;
        public Collection<SaveTagModel> TagModelCollection;

        public Collection<CharacterMark> CharaMarkCollection;
        public Collection<StoryFrameMark> StoryFrameMarkCollection;

        public Collection<CharacterStoryRelationModel> CharacterStoryRelationModelCollection;
        public Collection<ItemStoryRelationModel> ItemStoryRelationModelCollection;
        public Collection<TimelineEventModel> TimelineEventModelCollection;

        public DocumentModel DocumentModel;

        public NovelSettingModel NovelSettingModel;

        public XmlSaveObjectVer4_0_0()
        {
            CharacterModelCollection = new Collection<CharacterModel>();
            PlaceModelCollection = new Collection<PlaceModel>();
            StoryFrameModelCollection = new Collection<StoryFrameModel>();
            ItemModelCollection = new Collection<ItemModel>();
            TagModelCollection = new Collection<SaveTagModel>();

            CharaMarkCollection = new Collection<CharacterMark>();
            StoryFrameMarkCollection = new Collection<StoryFrameMark>();

            CharacterStoryRelationModelCollection = new Collection<CharacterStoryRelationModel>();
            ItemStoryRelationModelCollection = new Collection<ItemStoryRelationModel>();
            TimelineEventModelCollection = new Collection<TimelineEventModel>();

            DocumentModel = new DocumentModel();
            NovelSettingModel = new NovelSettingModel();
        }

        private void SetCharacterModel()
        {
            CharacterModelCollection = new Collection<CharacterModel>();

            var models = ModelsComposite.CharacterManager.ModelCollection;

            foreach (var mdl in models)
            {
                CharacterModelCollection.Add(mdl as CharacterModel);
            }
        }

        private void SetPlaceModel()
        {
            PlaceModelCollection = new Collection<PlaceModel>();

            var models = ModelsComposite.PlaceModelManager.ModelCollection;

            foreach (var mdl in models)
            {
                PlaceModelCollection.Add(mdl as PlaceModel);
            }
        }

        private void SetStoryFrameModel()
        {
            StoryFrameModelCollection = new Collection<StoryFrameModel>();

            var models = ModelsComposite.StoryFrameModelManager.ModelCollection;

            foreach (var mdl in models)
            {
                StoryFrameModelCollection.Add(mdl as StoryFrameModel);
            }
        }

        private void SetItemModel()
        {
            ItemModelCollection = new Collection<ItemModel>();

            var models = ModelsComposite.ItemModelManager.ModelCollection;

            foreach (var mdl in models)
            {
                ItemModelCollection.Add(mdl as ItemModel);
            }
        }

        private void SetMark()
        {
            CharaMarkCollection = new Collection<CharacterMark>();
            StoryFrameMarkCollection = new Collection<StoryFrameMark>();

            var marks = ModelsComposite.MarkManager.GetMarks();

            foreach (var mark in marks)
            {
                if (mark.GetType() == typeof(CharacterMark))
                {
                    CharaMarkCollection.Add(mark.Clone() as CharacterMark);
                }
                else if (mark.GetType() == typeof(StoryFrameMark))
                {
                    StoryFrameMarkCollection.Add(mark.Clone() as StoryFrameMark);
                }
            }
        }

        private void SetCharacterStoryRelation()
        {
            CharacterStoryRelationModelCollection = new Collection<CharacterStoryRelationModel>();

            foreach (var mgr in ModelsComposite.CharacterStoryModelManager.ModelCollection)
            {
                foreach (var model in mgr.ModelCollection)
                {
                    CharacterStoryRelationModelCollection.Add(model);
                }
            }
        }

        private void SetItemStoryRelation()
        {
            ItemStoryRelationModelCollection = new Collection<ItemStoryRelationModel>();

            foreach (var mgr in ModelsComposite.ItemStoryModelManager.ModelCollection)
            {
                foreach (var model in mgr.ModelCollection)
                {
                    ItemStoryRelationModelCollection.Add(model);
                }
            }
        }

        private void SetTagModel()
        {
            TagModelCollection = TagExpander.Expand(ModelsComposite.TagManager);
        }

        private void SetTimelineEvent()
        {
            TimelineEventModelCollection = new Collection<TimelineEventModel>();

            foreach (var model in ModelsComposite.TimelineEventModelManager.ModelCollection)
            {
                TimelineEventModelCollection.Add(model);
            }
        }

        public void AdjustMarksCausedByTextBoxBug()
        {
            while (true)
            {
                int index = DocumentModel.Text.IndexOf("\r");

                if (index == -1)
                {
                    break;
                }

                foreach (var mark in CharaMarkCollection)
                {
                    if (mark.HeadCharIndex >= index)
                    {
                        mark.HeadCharIndex--;
                    }
                    if (mark.TailCharIndex >= index)
                    {
                        mark.TailCharIndex--;
                    }
                }

                foreach (var mark in StoryFrameMarkCollection)
                {
                    if (mark.HeadCharIndex >= index)
                    {
                        mark.HeadCharIndex--;
                    }
                    if (mark.TailCharIndex >= index)
                    {
                        mark.TailCharIndex--;
                    }
                }

                DocumentModel.Text = DocumentModel.Text.Remove(index, 1);
            }

        }

        private void SetDocumentModel()
        {
            DocumentModel = new DocumentModel();
            DocumentModel.Text = ModelsComposite.DocumentModel.Text;
        }

        private void SetNovelSettingModel()
        {
            NovelSettingModel = ModelsComposite.NovelSettingModel;
        }

        public void SetSavableObject()
        {
            SetCharacterModel();
            SetPlaceModel();
            SetStoryFrameModel();
            SetItemModel();
            SetMark();
            SetCharacterStoryRelation();
            SetItemStoryRelation();
            SetTimelineEvent();
            SetDocumentModel();
            SetTagModel();
            SetNovelSettingModel();

            AdjustMarksCausedByTextBoxBug();
        }
    }
}
