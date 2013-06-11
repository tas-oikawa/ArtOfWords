using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernizedAlice.ArtOfWords.BizCommon.Model;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Marks;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Character;
using ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Item;
using System.Windows.Controls;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Relation;
using ModernizedAlice.ArtOfWords.BizCommon.Model.TimelineEvent;
using ModernizedAlice.IPlugin.ModuleInterface;
using ModernizedAlice.ArtOfWords.BizCommon.Model.Tag;

namespace ModernizedAlice.ArtOfWords.BizCommon
{
    public class ModelsComposite
    {
        static private CharacterManager _characterManager;
        static public CharacterManager CharacterManager
        {
            get
            {
                if (_characterManager == null)
                {
                    _characterManager = new CharacterManager();
                }
                return _characterManager;
            }
            set
            {
                _characterManager = value;
            }
        }

        static private StoryFrameModelManager _storyFrameModelManager;
        static public StoryFrameModelManager StoryFrameModelManager
        {
            get
            {
                if (_storyFrameModelManager == null)
                {
                    _storyFrameModelManager = new StoryFrameModelManager();
                }
                return _storyFrameModelManager;
            }
            set
            {
                _storyFrameModelManager = value;
            }
        }

        static private ItemModelManager _itemModelManager;
        static public ItemModelManager ItemModelManager
        {
            get
            {
                if (_itemModelManager == null)
                {
                    _itemModelManager = new ItemModelManager();
                }
                return _itemModelManager;
            }
            set
            {
                _itemModelManager = value;
            }
        }


        static private PlaceModelManager _placeModelManager;
        static public PlaceModelManager PlaceModelManager
        {
            get
            {
                if (_placeModelManager == null)
                {
                    _placeModelManager = new PlaceModelManager();
                }
                return _placeModelManager;
            }
            set
            {
                _placeModelManager = value;
            }
        }


        static private DocumentModel _documentModel;
        static public DocumentModel DocumentModel
        {
            get
            {
                if (_documentModel == null)
                {
                    _documentModel = new DocumentModel();
                }

                return _documentModel;
            }
            set
            {
                _documentModel = value;
            }
        }

        static private MarkManager _markManager;
        static public MarkManager MarkManager
        {
            get
            {
                if (_markManager == null)
                {
                    _markManager = new MarkManager();
                }

                return _markManager;
            }
            set
            {
                _markManager = value;
            }
        }

        static private CharacterStoryRelationModelManager _characterStoryModelManager;
        static public CharacterStoryRelationModelManager CharacterStoryModelManager
        {
            get
            {
                if (_characterStoryModelManager == null)
                {
                    _characterStoryModelManager = new CharacterStoryRelationModelManager();
                }

                return _characterStoryModelManager;
            }
            set
            {
                _characterStoryModelManager = value;
            }
        }

        static private ItemStoryRelationModelManager _itemStoryModelManager;
        static public ItemStoryRelationModelManager ItemStoryModelManager
        {
            get
            {
                if (_itemStoryModelManager == null)
                {
                    _itemStoryModelManager = new ItemStoryRelationModelManager();
                }

                return _itemStoryModelManager;
            }
            set
            {
                _itemStoryModelManager = value;
            }
        }

        private static TimelineEventModelManager _timelineEventModelManager;

        public static TimelineEventModelManager TimelineEventModelManager
        {
            get
            {
                if (_timelineEventModelManager == null)
                {
                    _timelineEventModelManager = new TimelineEventModelManager();
                }

                return ModelsComposite._timelineEventModelManager;
            }
            set { ModelsComposite._timelineEventModelManager = value; }
        }

        private static TagManager _tagManager;

        public static TagManager TagManager
        {
            get
            {
                if (_tagManager == null)
                {
                    _tagManager = new TagManager();
                    _tagManager.Add(new BaseTag());
                }

                return _tagManager;
            }
            set { ModelsComposite._tagManager = value; }
        }


        static public void CreateNew(IEditor iEditor)
        {
            ModelsComposite.DocumentModel = new Model.DocumentModel();

            iEditor.SetText(ModelsComposite.DocumentModel.Text);

            ModelsComposite.CharacterManager = new ModernizedAlice.ArtOfWords.BizCommon.Model.Character.CharacterManager();
            ModelsComposite.PlaceModelManager = new ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame.PlaceModelManager();
            ModelsComposite.StoryFrameModelManager = new ModernizedAlice.ArtOfWords.BizCommon.Model.StoryFrame.StoryFrameModelManager();
            ModelsComposite.ItemModelManager = new ModernizedAlice.ArtOfWords.BizCommon.Model.Item.ItemModelManager();
            ModelsComposite.MarkManager = new ModernizedAlice.ArtOfWords.BizCommon.Model.Marks.MarkManager();
            ModelsComposite.CharacterStoryModelManager = new CharacterStoryRelationModelManager();
            ModelsComposite.ItemStoryModelManager = new ItemStoryRelationModelManager();
            ModelsComposite.TimelineEventModelManager = new TimelineEventModelManager();
            ModelsComposite.TagManager = null;
        }

    }
}
