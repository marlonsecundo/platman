using Tilemap_Converter.Base;
using System;
using System.Xml;

namespace Tilemap_Converter
{
    public class Text : Layer
    {
        public Text() : base("textPoints")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();
            Bounds();
            Texture();
            Textt();
            Order();
        }

        private void Textt()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                itens[i].InnerXml = itens[i].InnerXml.Replace("type", "text");
            }
        }

        public override void Order()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                doc.LoadXml(itens[i].OuterXml);
                XmlNode position = doc.GetElementsByTagName("position")[0];
                XmlNode bounds = doc.GetElementsByTagName("bounds")[0];
                XmlNode texture = doc.GetElementsByTagName("texture")[0];
                XmlNode visible = doc.GetElementsByTagName("visible")[0];
                XmlNode text = doc.GetElementsByTagName("text")[0];

                itens[i].InnerXml = position.OuterXml + texture.OuterXml + visible.OuterXml + bounds.OuterXml + text.OuterXml;
            }
        }
    }
}
