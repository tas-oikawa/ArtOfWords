using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.Services.FileExpander
{
    public enum ExpandResult
    {
        Succeeded,
    }
    /// <summary>
    /// ファイルを拡張するInterface
    /// </summary>
    public interface FileExpanderInterface
    {
        /// <summary>
        /// 拡張する
        /// </summary>
        /// <param name="folderPath">フォルダーパス</param>
        /// <param name="iEditor">IEditor</param>
        /// <returns>拡張結果</returns>
        ExpandResult Expand(string folderPath, IEditor iEditor);
    }
}
