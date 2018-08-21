using System.Xml;
using Tilemap_Converter.Base;

namespace Tilemap_Converter.Screen
{
    public class Comp : Layer
    {
        public Comp() : base("comps")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();

            Order();

        }

        public override void Order()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                doc.LoadXml(itens[i].OuterXml);
                XmlNode position = doc.GetElementsByTagName("position")[0];

                itens[i].InnerXml = position.OuterXml;


            }
        }
    }
}
