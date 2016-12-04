using ModernizedAlice.ArtOfWords.BizCommon;
using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using ModernizedAlice.ArtOfWords.BizCommon.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace ModernizedAlice.ArtOfWords.Services.Manager
{
    /// <summary>
    /// 保存の結果
    /// </summary>
    public enum SaveResult
    {
        /// <summary>
        /// 成功
        /// </summary>
        Succeed,
        /// <summary>
        /// 失敗したがテキストだけは一時保存できた
        /// </summary>
        FailedButTextFile,
        /// <summary>
        /// 全部失敗　T(;_;)Tオテアゲェー
        /// </summary>
        CompletelyFailed,
    }

    /// <summary>
    /// 緊急保存モード
    /// </summary>
    /// <remarks>
    /// 保存に失敗したとき、とりあえず保存できそうなところに向かって保存をかける
    /// ProgramData -> ドキュメント -> デスクトップ
    /// </remarks>
    public enum SaveEmergencyMode
    {
        /// <summary>
        /// ProgramDataに保存
        /// </summary>
        AssemblyTemp,
        /// <summary>
        /// ドキュメントフォルダーに保存
        /// </summary>
        DocumentTemp,
        /// <summary>
        /// デスクトップに保存
        /// </summary>
        FinalTemp,
    }

    /// <summary>
    /// セーブを司るクラス
    /// </summary>
    public class SaveManager
    {
        /// <summary>
        /// 一時ファイルの名前を取得する
        /// </summary>
        /// <param name="path">本保存用ファイルのパス</param>
        /// <returns>一時ファイルの名前</returns>
        private String GetTempFileName(String path)
        {
            String fileName = Path.GetFileName(path);
            DateTime now = DateTime.Now;

            return "TMP" + fileName + now.Year + now.Month + now.Day + now.Hour + now.Minute + now.Second + now.Millisecond + ".tmp";
        }

        private String GetBackupFilePath(String path)
        {
            var dirPath = SaveUtil.GetBackupDirectoryPath();

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }


            return dirPath + "\\" + Path.GetFileName(path) + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private String GetTempFolderPath(SaveEmergencyMode mode)
        {
            switch (mode)
            {
                case SaveEmergencyMode.AssemblyTemp:
                    {
                        string assemblyLocation = CommonDirectoryUtil.GetCommonProgramDataAppliPath();

                        String dirPath = Path.GetDirectoryName(assemblyLocation);
                        return dirPath + "\\temp";
                    }
                case SaveEmergencyMode.DocumentTemp:
                    {
                        return System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    }
                case SaveEmergencyMode.FinalTemp:
                default:
                    {
                        return System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    }

            }
        }

        private SaveResult SaveTempText(String path, SaveEmergencyMode mode)
        {
            String dirPath = GetTempFolderPath(mode);
            String destPath = dirPath + "\\" + GetTempFileName(path);

            try
            {
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                StreamWriter stream = new StreamWriter(new FileStream(destPath, FileMode.CreateNew), Encoding.GetEncoding("Shift-jis"));
                stream.Write(ModelsComposite.DocumentModel.Text);
                stream.Close();
            }
            catch (Exception)
            {
                return SaveResult.CompletelyFailed;
            }

            return SaveResult.Succeed;
        }

        public SaveResult SaveHeader(string folderPath)
        {
            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(LoadFileInfo));
            var outstream = new System.IO.FileStream(folderPath + "\\" + "version.xml", System.IO.FileMode.Create);
            var saveInfo = new LoadFileInfo() { version = FileVersion.Ver4_0_0 };

            serializer.Serialize(outstream, saveInfo);
            outstream.Close();

            return SaveResult.Succeed;
        }

        public SaveResult SaveDocumentFile(String folderPath)
        {

            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(XmlSaveObjectVer4_0_0));
            // Tempフォルダーに書き出し隔離

            FileStream outstream = new System.IO.FileStream(folderPath + "\\" + "document.xml", System.IO.FileMode.Create);
            var saveObj = new XmlSaveObjectVer4_0_0();
            saveObj.SetSavableObject();

            serializer.Serialize(outstream, saveObj);
            outstream.Close();

            return SaveResult.Succeed;
        }

        public SaveResult Save(String path)
        {
            /*
            // 何が何でもテキストだけは保存する
            // 1. exeの直下にぶっ込む
            if (SaveTempText(path, SaveEmergencyMode.AssemblyTemp) == SaveResult.CompletelyFailed)
            {
                // 2. それも無理ならドキュメント直下にぶっ込む
                if (SaveTempText(path, SaveEmergencyMode.DocumentTemp) == SaveResult.CompletelyFailed)
                {
                    // 2. それも無理ならデスクトップ直下にぶっ込む
                    if (SaveTempText(path, SaveEmergencyMode.FinalTemp) == SaveResult.CompletelyFailed)
                    {
                        return SaveResult.CompletelyFailed;
                    }
                }
            }*/

            try
            {
                string tempFolder = Path.GetTempPath() + Path.GetRandomFileName();
                Directory.CreateDirectory(tempFolder);

                SaveHeader(tempFolder);
                SaveDocumentFile(tempFolder);

                CreateZip(path, tempFolder);

                File.Copy(path, GetBackupFilePath(path), true);

                Directory.Delete(tempFolder, true);
            }
            catch (Exception )
            {
                return SaveResult.FailedButTextFile;
            }

            return SaveResult.Succeed;
        }

        private void CreateZip(string destPath, string srcZipPath)
        {
            //ZipFileを作成する
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                
                zip.AlternateEncoding =
                    System.Text.Encoding.GetEncoding("shift_jis");
                
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                
                zip.UseZip64WhenSaving = Ionic.Zip.Zip64Option.AsNecessary;
                //エラーが出てもスキップする。デフォルトはThrow。
                zip.ZipErrorAction = Ionic.Zip.ZipErrorAction.Throw;
                zip.Comment = "donotopen";

                zip.Password = "ohaDNelson";
                zip.Encryption = Ionic.Zip.EncryptionAlgorithm.WinZipAes256;

                //フォルダを追加する
                zip.AddDirectory(srcZipPath);

                zip.Save(destPath);
            }
        }
    }
}
