using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernizedAlice.ArtOfWords.BizCommon.Model.SaveAndLoad
{
    public class SaveUtil
    {
        public static string GetBackupDirectoryPath()
        {
            string assemblyLocation = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\KienaiProject\\ArtOfWords\\";

            String dirPath = Path.GetDirectoryName(assemblyLocation) + "\\Backup";

            return dirPath;
        }

        public static string GetFileBoxConfigPath()
        {
            string assemblyLocation = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\KienaiProject\\ArtOfWords\\";

            String dirPath = Path.GetDirectoryName(assemblyLocation) + "\\FileBoxConfig.xml";

            return dirPath;
        }
    }
}
