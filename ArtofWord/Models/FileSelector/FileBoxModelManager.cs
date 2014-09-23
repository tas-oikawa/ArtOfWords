using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArtOfWords.Models.FileSelector
{
    /// <summary>
    /// ファイルボックスモデルのマネージャー
    /// </summary>
    public class FileBoxModelManager
    {
        private static RecentlyFileBoxModel _recentlyFileBoxModel;

        /// <summary>
        /// 直近使用したファイルのモデル
        /// </summary>
        public static RecentlyFileBoxModel RecentlyFileBoxModel
        {
            get
            {
                if (_recentlyFileBoxModel == null)
                {
                    _recentlyFileBoxModel = new RecentlyFileBoxModel();
                }
                return _recentlyFileBoxModel;
            }
        }

        /// <summary>
        /// 設定をロードする
        /// </summary>
        public static void Load()
        {
            var fileboxPath = SaveUtil.GetFileBoxConfigPath();
            if (!File.Exists(fileboxPath))
            {
                return;
            }

            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(RecentlyFileBoxModel));
            FileStream outstream = new System.IO.FileStream(fileboxPath, System.IO.FileMode.Open);

            _recentlyFileBoxModel = (RecentlyFileBoxModel)serializer.Deserialize(outstream);
            outstream.Close();
        }

        /// <summary>
        /// 設定をセーブする
        /// </summary>
        private static void Save()
        {
            var fileboxPath = SaveUtil.GetFileBoxConfigPath();
            if (!Directory.Exists(Path.GetDirectoryName(fileboxPath)))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileboxPath));
                }
                catch (Exception)
                {
                    return;
                }
            }

            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(RecentlyFileBoxModel));
            var outstream = new System.IO.FileStream(fileboxPath, System.IO.FileMode.Create);
            var saveInfo = RecentlyFileBoxModel;

            serializer.Serialize(outstream, saveInfo);
            outstream.Close();
        }

        /// <summary>
        /// 最近追加したファイルの一覧に、ファイルを追加する
        /// </summary>
        /// <param name="filePath"></param>
        public static void AddRecentlySavedFile(string filePath)
        {
            RecentlyFileBoxModel.AddRecentlyUsedFile(filePath);
            Save();
        }
    }
}
