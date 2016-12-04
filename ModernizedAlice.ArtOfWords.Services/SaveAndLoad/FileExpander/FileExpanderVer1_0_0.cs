using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.ArtOfWords.Services.FileExpander;
using ModernizedAlice.ArtOfWords.Services.ModelService;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ModernizedAlice.ArtOfWords.Services.FileExpander
{
    public class FileExpanderVer1_0_0 : FileExpanderInterface
    {
        private IEditor _iEditor;

        public XmlSaveObjectVer1_0_0 LoadComposition
        {
            get;
            set;
        }

        public ExpandResult Expand(string folderPath, IEditor iEditor)
        {
            _iEditor = iEditor;

            string fileCompositePath = folderPath + "\\document.xml";
            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(XmlSaveObjectVer1_0_0));
            FileStream outstream = new System.IO.FileStream(fileCompositePath, System.IO.FileMode.Open);

            LoadComposition = (XmlSaveObjectVer1_0_0)serializer.Deserialize(outstream);
            outstream.Close();

            ExpandObject();

            return ExpandResult.Succeeded;
        }


        private bool ExpandCharacter()
        {
            var service = new CharacterModelService();

            foreach (var model in LoadComposition.CharacterModelCollection)
            {
                service.AddCharacter(model);
            }

            return true;
        }
        private bool ExpandPlaceModel()
        {
            var service = new PlaceModelService();

            foreach (var model in LoadComposition.PlaceModelCollection)
            {
                service.AddPlace(model);
            }

            return true;
        }
        private bool ExpandStoryFrameModel()
        {
            var service = new StoryFrameModelService();

            foreach (var model in LoadComposition.StoryFrameModelCollection)
            {
                model.StartDateTime = DateTime.Today;
                model.EndDateTime = DateTime.Today;

                service.AddStoryFrame(model);
            }

            return true;
        }
        private bool ExpandItemModel()
        {
            var service = new ItemModelService();
            foreach (var model in LoadComposition.ItemModelCollection)
            {
                service.AddItem(model);
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

            return true;
        }
    }
}
