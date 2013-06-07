using ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.ViewModel.FileSelector
{
    public class RecentlyFileBoxModel
    {
        private List<string> _recentlyUsedFiles;

        public List<string> RecentlyUsedFiles
        {
            get { return _recentlyUsedFiles; }
            set { _recentlyUsedFiles = value; }
        }

        public RecentlyFileBoxModel()
        {
            _recentlyUsedFiles = new List<string>();
        }

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
