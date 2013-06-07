using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model
{
    /// <summary>
    /// マークすることが出来るオブジェクトが継承すべきインターフェイス
    /// </summary>
    public interface IMarkable
    {
        int Id { set; get; }
        String Symbol { set; get; }
        Brush ColorBrush { set; get; }

        bool IsValid { set; get; }
    }
}
