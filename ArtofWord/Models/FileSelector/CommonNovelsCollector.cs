using FileSelector.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfWords.Models.FileSelector
{
    /// <summary>
    /// ただのコレクションクラス
    /// </summary>
    /// <remarks>いらない気がする？</remarks>
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
