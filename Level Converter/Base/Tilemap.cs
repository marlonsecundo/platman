using System.Collections.Generic;
using System.Xml;

namespace Tilemap_Converter.Base
{
    public abstract class Tilemap
    {
        List<Layer> layers;

        XmlDocument docu = new XmlDocument();

        public abstract XmlDocument Convert(string xml);

        protected virtual void RenameLayers(ref XmlDocument doc)
        {
            XmlNodeList list = doc.GetElementsByTagName("layers");

            XmlNode[] layer = new XmlNode[list.Count];

            for (int i = 0; i < layer.Length; i++)
                layer[i] = list[i];

            docu.LoadXml(layer[0].OuterXml);

            var parent = layer[0].ParentNode;

            for (int i = 0; i < layer.Length; i++)
            {
                layer[i].InnerXml = layer[i].InnerXml.Replace("objects", "Item");

                docu.LoadXml(layer[i].OuterXml);


                string name = docu.GetElementsByTagName("name")[0].InnerText;

                list = docu.GetElementsByTagName("Item");
                XmlNode[] itens = new XmlNode[list.Count];
                for (int j = 0; j < itens.Length; j++)
                    itens[j] = list[j];


                {
                    string element = "<" + name + ">";

                    for (int j = 0; j < itens.Length; j++)
                        element += itens[j].OuterXml;

                    element += "</" + name + ">";

                    parent.InnerXml += element;
                }
            }

            Util.RemoveTags(ref doc, new string[] { "layers" });
        }

        protected virtual void Bounds(ref XmlDocument doc)
        {
            XmlNode heightNode = doc.GetElementsByTagName("height")[0];
            XmlNode widthNode = doc.GetElementsByTagName("width")[0];

            var parent = heightNode.ParentNode;

            string height = int.Parse(heightNode.InnerText) * 30 + "";
            string width = int.Parse(widthNode.InnerText) * 30 + "";

            heightNode.ParentNode.RemoveChild(heightNode);
            widthNode.ParentNode.RemoveChild(widthNode);

            parent.InnerXml = "<bounds> 0 0 " + width + " " + height + "</bounds>" + parent.InnerXml;
        }

        protected void RemoveTags(ref XmlDocument doc)
        {
            Util.RemoveTags(ref doc, new string[] { "gid", "compsBackgorund", "draworder", "nextobjectid", "orientation", "renderorder", "tiledversion", "tileheight", "firstgid", "source", "tilesets", "tilewidth", "version", "id" });
            Util.RemoveTags(ref doc, new string[] { "tilesets", "source", "opacity", "rotation", "infinite" });

            XmlNode node = doc.GetElementsByTagName("type")[0];
            node.ParentNode.RemoveChild(node);
        }
    }
}
