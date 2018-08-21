using System.Xml;
using Tilemap_Converter.Base;

namespace Tilemap_Converter.Screen
{
    public class Textt : Layer
    {
        public Textt() : base("texts")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();

            Size();

            Texti();

            Order();
        }

        private void Texti()
        {
            for (int i = 0; i < itens.Length; i++)
            {

                doc.LoadXml(itens[i].OuterXml);

                if (i == 14)
                {

                }

                // PECURIALIDADE: TEM DUAS TAG COM O NOME TEXT, UMA É PAI E OUTRA É FILHA
                doc.LoadXml("<root>"+doc.GetElementsByTagName("text")[0].InnerXml+"</root>");

                string text = doc.GetElementsByTagName("text")[0].InnerText;

                itens[i].InnerXml = itens[i].InnerXml.Replace("text", "old");

                switch (text)
                {
                    case "<": text = "&lt;"; break;
                    case ">": text = "&gt;"; break;
                }

                itens[i].InnerXml += "<text>" + text + "</text>";

            }
        }

        private void Size()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                doc.LoadXml(itens[i].OuterXml);

                string size = doc.GetElementsByTagName("pixelsize")[0].InnerText;

                string fontSize = "Anyone";

                switch(size)
                {
                    case "60": fontSize = "ExtraBig"; break;
                    case "56": fontSize = "Big"; break;
                    case "40": fontSize = "Middle"; break;
                    case "32": fontSize = "MiddleSmall"; break;
                    case "18": fontSize = "Small"; break;
                }

                itens[i].InnerXml += "<size>"+fontSize+"</size>";
                
            }
        }

        public override void Order()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                doc.LoadXml(itens[i].OuterXml);

                XmlNode position = doc.GetElementsByTagName("position")[0];
                XmlNode size = doc.GetElementsByTagName("size")[0];
                XmlNode text = doc.GetElementsByTagName("text")[0];

                itens[i].InnerXml = position.OuterXml + size.OuterXml + text.OuterXml;
            }
        }
    }
}
