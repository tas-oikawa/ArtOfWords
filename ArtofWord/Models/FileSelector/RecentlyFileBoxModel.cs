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
    /// 最近使用したファイルボックスのモデル
    /// </summary>
    public class RecentlyFileBoxModel
    {
        private List<string> _recentlyUsedFiles;

        /// <summary>
        /// 最近使用したファイルの一覧
        /// </summary>
        public List<string> RecentlyUsedFiles
        {
            get { return _recentlyUsedFiles; }
            set { _recentlyUsedFiles = value; }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public RecentlyFileBoxModel()
        {
            _recentlyUsedFiles = new List<string>();
        }

        /// <summary>
        /// 最近使用したファイルのリストにファイルを追加する
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        public void AddRecentlyUsedFile(string path)
        {
            if (_recentlyUsedFiles.Contains(path))
            {
                _recentlyUsedFiles.Remove(path);
            }

            if (_recentlyUsedFiles.Count == 10)
            {
                _recentlyUsedFiles.RemoveAt(9);
            }

            _recentlyUsedFiles.Insert(0, path);
        }

    }
}
