using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad
{
    public class FileExpanderVer3_0_0 : FileExpanderInterface
    {
        private IEditor _iEditor;

        public XmlSaveObjectVer3_0_0 LoadComposition
        {
            get;
            set;
        }

        public ExpandResult Expand(string folderPath, IEditor iEditor)
        {
            _iEditor = iEditor;

            string fileCompositePath = folderPath + "\\document.xml";
            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(XmlSaveObjectVer3_0_0));
            FileStream outstream = new System.IO.FileStream(fileCompositePath, System.IO.FileMode.Open);

            LoadComposition = (XmlSaveObjectVer3_0_0)serializer.Deserialize(outstream);
            outstream.Close();

            ExpandObject();

            return ExpandResult.Succeeded;
        }


        private bool ExpandCharacter()
        {
            foreach (var model in LoadComposition.CharacterModelCollection)
            {
                ModelsComposite.CharacterManager.AddModel(model);
            }

            return true;
        }
        private bool ExpandPlaceModel()
        {
            foreach (var model in LoadComposition.PlaceModelCollection)
            {
                ModelsComposite.PlaceModelManager.AddModel(model);
            }

            return true;
        }
        private bool ExpandStoryFrameModel()
        {
            foreach (var model in LoadComposition.StoryFrameModelCollection)
            {
                ModelsComposite.StoryFrameModelManager.AddModel(model);
                ModelsComposite.CharacterStoryModelManager.AddStoryFrameModel(model.Id);
                ModelsComposite.ItemStoryModelManager.AddStoryFrameModel(model.Id);
            }

            return true;
        }
        private bool ExpandItemModel()
        {
            foreach (var model in LoadComposition.ItemModelCollection)
            {
                ModelsComposite.ItemModelManager.AddModel(model);
            }

            return true;
        }

        private bool ExpandCharacterMark()
        {
            foreach (var model in LoadComposition.CharaMarkCollection)
            {
                model.Parent = ModelsComposite.CharacterManager.FindCharacter(model.CharacterId);
                ModelsComposite.MarkManager.AddMark(model);
            }

            return true;
        }

        private bool ExpandStoryFrameMark()
        {
            foreach (var model in LoadComposition.StoryFrameMarkCollection)
            {
                model.Parent = ModelsComposite.StoryFrameModelManager.FindStoryFrameModel(model.MarkId);
                ModelsComposite.MarkManager.AddMark(model);
            }

            return true;
        }

        private bool ExpandCharacterStoryRelationModel()
        {
            foreach (var model in LoadComposition.CharacterStoryRelationModelCollection)
            {
                model.DoActionAfterLoad();
                var oneMgr = ModelsComposite.CharacterStoryModelManager.FindModel(model.StoryFrameId);
                oneMgr.AddModel(model);
            }

            return true;
        }

        private bool ExpandItemStoryRelationModel()
        {
            foreach (var model in LoadComposition.ItemStoryRelationModelCollection)
            {
                model.DoActionAfterLoad();
                var oneMgr = ModelsComposite.ItemStoryModelManager.FindModel(model.StoryFrameId);
                oneMgr.AddModel(model);
            }

            return true;
        }

        private bool ExpandTagModel()
        {
            TagExpander.Expand(LoadComposition.TagModelCollection,ModelsComposite.TagManager);
         
            return true;
        }

        private bool ExpandTimelineEventModel()
        {
            foreach (var model in LoadComposition.TimelineEventModelCollection)
            {
                ModelsComposite.TimelineEventModelManager.AddModel(model);
            }

            return true;
        }


        public bool ExpandObject()
        {
            ModelsComposite.CreateNew(_iEditor);

            _iEditor.SetText(LoadComposition.DocumentModel.Text);

            ModelsComposite.DocumentModel = LoadComposition.DocumentModel;
            ExpandCharacter();
            ExpandPlaceModel();
            ExpandStoryFrameModel();
            ExpandItemModel();
            ExpandCharacterMark();
            ExpandStoryFrameMark();
            ExpandCharacterStoryRelationModel();
            ExpandItemStoryRelationModel();
            ExpandTagModel();
            ExpandTimelineEventModel();
            ModelsComposite.NovelSettingModel = LoadComposition.NovelSettingModel;

            return true;
        }
    }
}
