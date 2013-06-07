using ModernizedAlice.IPlugin.ModuleInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad
{
    public enum ExpandResult
    {
        Succeeded,
    }

    public interface FileExpanderInterface
    {
        ExpandResult Expand(string folderPath, IEditor iEditor);
    }
}
