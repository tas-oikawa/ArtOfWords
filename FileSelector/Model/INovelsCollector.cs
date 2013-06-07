using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSelector.Model
{
    public interface INovelsCollector
    {
        List<NovelFileModel> Get();
    }
}
