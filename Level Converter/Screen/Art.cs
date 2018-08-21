using System.Xml;
using Tilemap_Converter.Base;

namespace Tilemap_Converter.Screen
{
    public class Art : Layer
    {
        public Art() : base("arts")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();

            Animations();

            Order();
        }

        private void Animations()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                doc.LoadXml(itens[i].OuterXml);
                string name = doc.GetElementsByTagName("name")[0].InnerText;
                itens[i].InnerXml += "<animations><Item><key>key1</key><texture>"+name+"</texture>"+"<frameCount></frameCount><rows></rows><columns></columns><delay>100</delay><repeat>true</repeat></Item></animations>";
            }
        }

        public override void Order()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                doc.LoadXml(itens[i].OuterXml);

                XmlNode position = doc.GetElementsByTagName("position")[0];
                XmlNode animarions = doc.GetElementsByTagName("animations")[0];

                itens[i].InnerXml = position.OuterXml + animarions.OuterXml;
            }
        }
    }
}
