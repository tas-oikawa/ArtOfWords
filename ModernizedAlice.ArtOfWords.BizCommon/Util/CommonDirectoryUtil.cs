using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Util
{
    /// <summary>
    /// 共通DirectoryのためのUtil
    /// </summary>
    public static class CommonDirectoryUtil
    {
        /// <summary>
        /// 共通ProgramDataのヴェンダーPathを取得
        /// </summary>
        /// <returns>
        /// 共通ProgramDataのPath
        /// </returns>
        public static string GetCommonProgramDataVendorPath()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\KienaiProject\\";
        }


        /// <summary>
        /// 共通ProgramDataのアプリケーションPathを取得
        /// </summary>
        /// <returns>
        /// 共通ProgramDataのPath
        /// </returns>
        public static string GetCommonProgramDataAppliPath()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\KienaiProject\\ArtOfWords\\";
        }

        /// <summary>
        /// AdsDirectoryのためのパスを取得する
        /// </summary>
        /// <returns>
        /// AdsDirectoryのPath
        /// </returns>
        public static string GetAdsDirectoryPath()
        {
            return GetCommonProgramDataAppliPath() + "AdsOfWorld\\";
        }

        ///保存系のパスはSaveUtilに委譲
    }
}
