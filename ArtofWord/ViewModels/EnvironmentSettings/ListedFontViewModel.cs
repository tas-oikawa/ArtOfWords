using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ArtOfWords.ViewModels.EnvironmentSettings
{
    /// <summary>
    /// リスト用のフォント
    /// </summary>
    public class ListedFontViewModel
    {
        private string _fontName;

        /// <summary>
        /// フォント名
        /// </summary>
        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        private string _sampleString;

        /// <summary>
        /// 文言例
        /// </summary>
        public string SampleString
        {
            get { return _sampleString; }
            set { _sampleString = value; }
        }


        private FontFamily _fontFamily;

        /// <summary>
        /// フォントファミリー
        /// </summary>
        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set { _fontFamily = value; }
        }
    }
}
