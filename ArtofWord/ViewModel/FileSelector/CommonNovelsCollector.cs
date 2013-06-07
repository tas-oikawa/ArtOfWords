using FileSelector.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.ViewModel.FileSelector
{
    public class CommonNovelsCollector : INovelsCollector
    {
        public CommonNovelsCollector()
        {
            Novels = new List<NovelFileModel>();
        }
        public List<NovelFileModel> Novels;

        public List<NovelFileModel> Get()
        {
            return Novels;
        }
    }
}
