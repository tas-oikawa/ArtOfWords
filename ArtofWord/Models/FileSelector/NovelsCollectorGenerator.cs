using FileSelector.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.Models.FileSelector
{
    /// <summary>
    /// NovelCollectorを生成するクラス
    /// </summary>
    public class NovelsCollectorGenerator
    {
        /// <summary>
        /// 直近使用した小説のCollectorを生成する
        /// </summary>
        public static INovelsCollector GetRecentlyNovelsCollector()
        {
            var collector = new CommonNovelsCollector();

            foreach (var file in FileBoxModelManager.RecentlyFileBoxModel.RecentlyUsedFiles)
            {
                if(!File.Exists(file))
                {
                    continue;
                }

                collector.Novels.Add(new NovelFileModel()
                {
                    FileName = Path.GetFileNameWithoutExtension(file),
                    FilePath = file,
                    UpdateDateTime = File.GetLastWriteTime(file),
                }
                );
            }
            return collector;
        }

        /// <summary>
        /// 日付比較用のComparer
        /// </summary>
        public class UpdateTimeDescComparer : IComparer<NovelFileModel>
        {
            public int Compare(NovelFileModel x, NovelFileModel y)
            {
                if (y.UpdateDateTime > x.UpdateDateTime)
                {
                    return 1;
                }
                else if (y.UpdateDateTime < x.UpdateDateTime)
                {
                    return -1;
                }
                return 0;
            }
        }

        /// <summary>
        /// セミオートバックアップのCollectorを生成する
        /// </summary>
        /// <returns></returns>
        public static INovelsCollector GetSemiAutoBackupNovelsCollector()
        {
            BackupFilesManager.RecollectBackups();
            var backups = BackupFilesManager.GetBackupFiles();

            var collector = new CommonNovelsCollector();

            foreach (var file in backups)
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                collector.Novels.Add(new NovelFileModel()
                {
                    FileName = Path.GetFileNameWithoutExtension(file),
                    FilePath = file,
                    UpdateDateTime = File.GetLastWriteTime(file),
                }
                );
            }

            collector.Novels.Sort(new UpdateTimeDescComparer());

            return collector;
        }

        /// <summary>
        /// ノベルズボックスのコレクターを作成する
        /// </summary>
        public static INovelsCollector GetNovelsBoxCollector()
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly().Location ;
            var novelsDir = Path.GetDirectoryName(asm) + "\\Novels";

            var collector = new CommonNovelsCollector();

            if (!Directory.Exists(novelsDir))
            {
                return collector;
            }

            foreach (var file in Directory.GetFiles(novelsDir))
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                collector.Novels.Add(new NovelFileModel()
                {
                    FileName = Path.GetFileNameWithoutExtension(file),
                    FilePath = file,
                    UpdateDateTime = File.GetLastWriteTime(file),
                }
                );
            }

            return collector;
        }
    }
}
