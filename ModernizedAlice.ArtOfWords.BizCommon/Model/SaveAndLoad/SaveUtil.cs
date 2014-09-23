using ModernizedAlice.ArtOfWords.BizCommon.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad
{
    /// <summary>
    /// 保存に使うユーティリティクラス
    /// </summary>
    public class SaveUtil
    {
        /// <summary>
        /// バックアップディレクトリのパスを取得する
        /// </summary>
        /// <returns>バックアップディレクトリのパス</returns>
        public static string GetBackupDirectoryPath()
        {
            string assemblyLocation = CommonDirectoryUtil.GetCommonProgramDataAppliPath();

            String dirPath = Path.GetDirectoryName(assemblyLocation) + "\\Backup";

            return dirPath;
        }

        /// <summary>
        /// ファイルコンフィグのパスを取得する
        /// </summary>
        /// <returns>ファイルコンフィグのパス</returns>
        public static string GetFileBoxConfigPath()
        {
            string assemblyLocation = CommonDirectoryUtil.GetCommonProgramDataAppliPath();

            String dirPath = Path.GetDirectoryName(assemblyLocation) + "\\FileBoxConfig.xml";

            return dirPath;
        }
    }
}
