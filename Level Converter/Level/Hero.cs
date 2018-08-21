using Tilemap_Converter.Base;
using System.Xml;

namespace Tilemap_Converter
{
    public class Hero : Layer
    {
        public Hero() : base("hero")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();

            Order();
        }

        protected override void Position()
        {
            base.Position();

            doc.LoadXml(itens[0].OuterXml);

            XmlNode node = doc.GetElementsByTagName("position")[0];

            itens[0].ParentNode.InnerXml = node.OuterXml;
        }

        public override void Order()
        {

        }
    }
}
