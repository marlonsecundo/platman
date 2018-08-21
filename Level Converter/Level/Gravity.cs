using Tilemap_Converter.Base;
using System.Xml;

namespace Tilemap_Converter
{
    public class Gravity : Layer
    {
        public Gravity() : base("gravityPoints")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();
            Bounds();
            Texture();
            Orientation();
            Order();
        }

        private void Orientation()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                itens[i].InnerXml = itens[i].InnerXml.Replace("type", "orientation");
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
                XmlNode orientation = doc.GetElementsByTagName("orientation")[0];

                itens[i].InnerXml = position.OuterXml + texture.OuterXml + visible.OuterXml + bounds.OuterXml + orientation.OuterXml;
            }
        }
    }
}
