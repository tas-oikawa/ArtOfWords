using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.Models.FileSelector
{
    /// <summary>
    /// バックアップファイルの管理者
    /// </summary>
    public class BackupFilesManager
    {

        /// <summary>
        /// セミオートバックアップファイルの属性
        /// </summary>
        private class SemiAutoSaveFileAttr
        {
            /// <summary>
            /// ファイルのフルパス
            /// </summary>
            public string FullPath;
            /// <summary>
            /// ファイル名
            /// </summary>
            public string FileName;
            /// <summary>
            /// 作成日時
            /// </summary>
            public DateTime CreatedDate;
        }

        /// <summary>
        /// バックアップファイルを取得する
        /// </summary>
        /// <returns>バックアップファイルの一覧</returns>
        public static List<String> GetBackupFiles()
        {
            List<String> backupFiles = new List<string>();

            var backupDir = SaveUtil.GetBackupDirectoryPath();

            if (!Directory.Exists(backupDir))
            {
                return backupFiles;
            }

            foreach (var file in Directory.EnumerateFiles(backupDir))
            {
                backupFiles.Add(file);
            }

            return backupFiles;
        }

        /// <summary>
        /// セミオートバックアップファイルの属性を取得してディクショナリー形式にする
        /// </summary>
        /// <param name="backupFiles">バックアップファイルの一覧</param>
        /// <returns>キー：バックアップファイル名, 値：バックアップファイルの属性</returns>
        private static Dictionary<string, List<SemiAutoSaveFileAttr>> GetSemiAutoSaveFileDic(List<string> backupFiles)
        {
            var dic = new Dictionary<string, List<SemiAutoSaveFileAttr>>();

            foreach (var backupFile in backupFiles)
            {
                var attr = GetAttr(backupFile);
                if (attr == null)
                {
                    continue;
                }

                if (!dic.ContainsKey(attr.FileName))
                {
                    dic.Add(attr.FileName, new List<SemiAutoSaveFileAttr>());
                }
                dic[attr.FileName].Add(attr);
            }

            return dic;
        }

        /// <summary>
        /// バックアップファイルをソートする
        /// </summary>
        private static IOrderedEnumerable<SemiAutoSaveFileAttr> SortBackups(List<SemiAutoSaveFileAttr> target)
        {
            return target.OrderByDescending(x => x.CreatedDate);
        }

        /// <summary>
        /// 期限切れのバックアップファイルを削除する
        /// </summary>
        private static void RemoveExpiredBackups(IOrderedEnumerable<SemiAutoSaveFileAttr> orderedList)
        {
            int found = 0;
            foreach (var item in orderedList)
            {
                found++;

                // 直近五個は保管しておく
                if (found <= 6)
                {
                    continue;
                }

                // 作成後五日後以内のものは残す
                if (item.CreatedDate.AddDays(5) > DateTime.Today)
                {
                    continue;
                }

                if (File.Exists(item.FullPath) == false)
                {
                    continue;
                }

                try
                {
                    File.Delete(item.FullPath);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 過去のバックアップファイルを整理する
        /// </summary>
        public static void RecollectBackups()
        {
            var backups = GetBackupFiles();

            var dic = GetSemiAutoSaveFileDic(backups);

            foreach (var key in dic.Keys)
            {
                var list = dic[key];
                RemoveExpiredBackups(SortBackups(list));
            }
        }

        /// <summary>
        /// ファイル名から属性を抽出する
        /// </summary>
        private static SemiAutoSaveFileAttr GetAttr(string path)
        {
            string fileName = Path.GetFileName(path);

            const int DATE_TIME_LENGTH = 14;

            if (fileName == null || fileName.Count() <= DATE_TIME_LENGTH + 1)
            {
                return null;
            }

            if (!fileName.Contains(".") || fileName.IndexOf(".") == 0)
            {
                return null;
            }

            string dateStr = fileName.Substring(fileName.Count() - DATE_TIME_LENGTH, DATE_TIME_LENGTH);

            DateTime date;
            if (!DateTime.TryParseExact(dateStr, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out date))
            {
                return null;
            }

            string fileNameWithoutExtension = fileName.Substring(0, fileName.IndexOf("."));

            return new SemiAutoSaveFileAttr() { CreatedDate = date, FileName = fileNameWithoutExtension, FullPath = path };
        }
    }
}
