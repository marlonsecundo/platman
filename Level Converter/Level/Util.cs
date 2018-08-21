using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Xml;

namespace Tilemap_Converter
{
    public static class Util
    {
        public static void RemoveTags(ref XmlDocument document, string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                var list = document.GetElementsByTagName(names[i]);

                XmlNode[] nodes = new XmlNode[list.Count];

                for (int j = 0; j < nodes.Length; j++)
                    nodes[j] = list[j];

                for (int j = 0; j < nodes.Length; j++)                
                    nodes[j].ParentNode.RemoveChild(nodes[j]);
            }
        }
        public static void SaveFileXml(XmlDocument xmlDoc, string dir, string filename)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Xml Document(*.xml) | *.xml";
            saveFileDialog1.FileName = filename + ".xml";
            saveFileDialog1.DefaultExt = ".xml";
            saveFileDialog1.RestoreDirectory = false;
            saveFileDialog1.InitialDirectory = dir;

            var result = saveFileDialog1.ShowDialog();

            if (result == true)
            {
                using (Stream s = saveFileDialog1.OpenFile())
                {
                    using (XmlTextWriter xmlWriter = new XmlTextWriter(s, Encoding.UTF8))
                    {
                        xmlWriter.Formatting = System.Xml.Formatting.Indented;
                        xmlDoc.WriteContentTo(xmlWriter);
                    }
                }
            }
        }
    }
}
