using Ionic.Zip;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.ArtOfWords.Services.FileExpander;
using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ModernizedAlice.ArtOfWords.Services.Manager
{
    public enum LoadResult
    {
        Succeed,
        Failed,
    }
    public class LoadManager
    {
        private IEditor _iEditor;

        public LoadManager(IEditor iEditor)
        {
            _iEditor = iEditor;
        }

        public LoadFileInfo LoadFileInfo
        {
            get;
            set;
        }

        public void LoadFileInfoFile(string folderPath)
        {
            string versionFilePath = folderPath + "\\version.xml";

            if (!File.Exists(versionFilePath))
            {
                LoadFileInfo = new LoadFileInfo()
                {
                    version = FileVersion.Ver1_0_0
                };

                return ;
            }

            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(LoadFileInfo));
            FileStream outstream = new System.IO.FileStream(versionFilePath, System.IO.FileMode.Open);

            LoadFileInfo = (LoadFileInfo)serializer.Deserialize(outstream);
            outstream.Close();
        }

        public LoadResult Load(String path)
        {
            try
            {
                string folderPath;
                OpenZip(path, out folderPath);
                LoadFileInfoFile(folderPath);
                var expander = FileExpanderFactory.GetExpander(LoadFileInfo);

                if (expander == null)
                {
                    Directory.Delete(Path.GetDirectoryName(folderPath), true);
                    throw new Exception("このファイルは展開できません");
                }

                expander.Expand(folderPath, _iEditor);

                Directory.Delete(folderPath, true);
            }
            catch (Exception )
            {
                return LoadResult.Failed;
            }

            return LoadResult.Succeed;
        }

        private bool OpenZip(String path, out string outpath)
        {
            //ZipFileを作成する
            using (Ionic.Zip.ZipFile zip = ZipFile.Read(path, new ReadOptions(){Encoding = Encoding.GetEncoding("shift_jis")}))
            {
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;

                zip.ExtractExistingFile = Ionic.Zip.ExtractExistingFileAction.OverwriteSilently;
                //エラーが出てもスキップする。デフォルトはThrow。
                zip.ZipErrorAction = Ionic.Zip.ZipErrorAction.Skip;

                zip.Password = "ohaDNelson";
                zip.Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes256;

                // Tempフォルダーに書き出し隔離
                string tempFolder = Path.GetTempPath() + Path.GetRandomFileName();
                Directory.CreateDirectory(tempFolder);

                //フォルダを追加する
                zip.ExtractAll(tempFolder);

                outpath = tempFolder;
            }

            return true;
        }
    }
}
