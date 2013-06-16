using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad
{
    public class FileExpanderFactory
    {
        public static FileExpanderInterface GetExpander(LoadFileInfo loadFileInfo)
        {
            if (loadFileInfo.version == FileVersion.Ver1_0_0)
            {
                return new FileExpanderVer1_0_0();
            }
            if (loadFileInfo.version == FileVersion.Ver2_0_0)
            {
                return new FileExpanderVer2_0_0();
            }
            if (loadFileInfo.version == FileVersion.Ver3_0_0)
            {
                return new FileExpanderVer3_0_0();
            }
            return null;
        }
    }
}
