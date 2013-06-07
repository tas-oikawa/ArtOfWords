using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ArtOfWords.Salesman
{
    public class AdsXmlFormatManager
    {
        private string _version = "";

        public AdsXmlFormatManager(string version)
        {
            _version = version;
        }

        public AdsXmlFormat GetXmlFormat(string xmlPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<AdsXmlFormat>));
            FileStream outstream = new System.IO.FileStream(xmlPath, System.IO.FileMode.Open);

            var adsXmlCollection = (List<AdsXmlFormat>)serializer.Deserialize(outstream);
            outstream.Close();

            AdsXmlFormat def = null;
            foreach (var ads in adsXmlCollection)
            {
                if (ads.Version == _version)
                {
                    return ads;
                }
                if (ads.Version == "Default")
                {
                    def = ads;
                }
            }

            return def;
        }

        public void Generate(string destPath, List<AdsXmlFormat> src)
        {
            // ちゃんとしたファイルを書き出す。
            XmlSerializer serializer = new XmlSerializer(typeof(List<AdsXmlFormat>));
            var outstream = new System.IO.FileStream(destPath, System.IO.FileMode.Create);
            var saveInfo = src;

            serializer.Serialize(outstream, saveInfo);
            outstream.Close();
        }
    }
}
