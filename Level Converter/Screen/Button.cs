using System;
using System.Xml;
using Tilemap_Converter.Base;

namespace Tilemap_Converter.Screen
{
    public class Button : Layer
    {
        public Button() : base("buttons")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();

            Key();

            Delay();

            Order();
        }

        private void Key()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                itens[i].InnerXml = itens[i].InnerXml.Replace("type", "key");
            }
        }

        private void Delay()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                itens[i].InnerXml += "<delay>100</delay>";
            }
        }

        public override void Order()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                doc.LoadXml(itens[i].OuterXml);

                XmlNode position = doc.GetElementsByTagName("position")[0];
                XmlNode name = doc.GetElementsByTagName("name")[0];
                XmlNode delay = doc.GetElementsByTagName("delay")[0];
                XmlNode key = doc.GetElementsByTagName("key")[0];

                itens[i].InnerXml = position.OuterXml + name.OuterXml + delay.OuterXml + key.OuterXml;
            }
        }
    }
}
