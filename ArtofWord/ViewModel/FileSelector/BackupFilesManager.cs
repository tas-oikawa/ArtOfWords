using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.ViewModel.FileSelector
{
    public class BackupFilesManager
    {

        class SemiAutoSaveFileAttr
        {
            public string FullPath;
            public string FileName;
            public DateTime CreatedDate;
        }

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

        private static IOrderedEnumerable<SemiAutoSaveFileAttr> SortBackups(List<SemiAutoSaveFileAttr> target)
        {
            return target.OrderByDescending(x => x.CreatedDate);
        }

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

                //　五日以上たったものは削除
                if (item.CreatedDate.AddDays(5) <= DateTime.Today)
                {
                    if (File.Exists(item.FullPath))
                    {
                        try
                        {
                            File.Delete(item.FullPath);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        public static void ReorderBackups()
        {
            var backups = GetBackupFiles();

            var dic = GetSemiAutoSaveFileDic(backups);

            foreach (var key in dic.Keys)
            {
                var list = dic[key];
                RemoveExpiredBackups(SortBackups(list));
            }
        }

        private static SemiAutoSaveFileAttr GetAttr(string path)
        {
            string fileName = Path.GetFileName(path);

            const int dateTimeLength = 14;
            if (fileName == null || fileName.Count() <= dateTimeLength + 1)
            {
                return null;
            }
            if (!fileName.Contains(".") || fileName.IndexOf(".") == 0)
            {
                return null;
            }

            string dateStr = fileName.Substring(fileName.Count() - dateTimeLength, dateTimeLength);

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
