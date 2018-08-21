using System;
using System.Xml;

namespace Tilemap_Converter.Base
{
    public abstract class Layer
    {
        protected XmlDocument doc = new XmlDocument();
        protected XmlNode[] itens;
        protected XmlNode parent;
        string name;

        public Layer(string name)
        {
            this.name = name;
        }

        public virtual void Convert(ref XmlDocument doc)
        {
            parent = doc.GetElementsByTagName(name)[0];
            itens = new XmlNode[parent.ChildNodes.Count];

            for (int i = 0; i < itens.Length; i++)
                itens[i] = parent.ChildNodes[i];
        }

        protected virtual void Position()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                double x = 0;
                double y = 0;

                doc.LoadXml(itens[i].OuterXml);

                XmlNode nodeX = doc.GetElementsByTagName("x")[0];
                XmlNode nodeY = doc.GetElementsByTagName("y")[0];

                string height = doc.GetElementsByTagName("height")[0].InnerText.Replace(".",",");
                string width = doc.GetElementsByTagName("width")[0].InnerText.Replace(".",",");

                x = double.Parse(nodeX.InnerText.Replace(".",","));
                y = double.Parse(nodeY.InnerText.Replace(".",",")) - double.Parse(height);

                x += double.Parse(width) / 2;
                y += double.Parse(height) / 2;

                x = Math.Round(x, 1);
                y = Math.Round(y, 1);

                doc.FirstChild.RemoveChild(nodeX);
                doc.FirstChild.RemoveChild(nodeY);
               

                itens[i].InnerXml = ("<position>" + x + " " + y + "</position>" + doc.FirstChild.InnerXml).Replace(",",".");
            }
        }

        protected void Texture()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                itens[i].InnerXml = itens[i].InnerXml.Replace("name", "texture");
            }
        }

        protected virtual void Bounds()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                string height = "";
                string width = "";

                doc.LoadXml(itens[i].OuterXml);

                XmlNode nodeHeight = doc.GetElementsByTagName("height")[0];
                XmlNode nodeWidth = doc.GetElementsByTagName("width")[0];

                height = nodeHeight.InnerText;
                width = nodeWidth.InnerText;

                doc.FirstChild.RemoveChild(nodeHeight);
                doc.FirstChild.RemoveChild(nodeWidth);

                doc.FirstChild.InnerXml += "<bounds> 0 0 " + width + " " + height + "</bounds>";

                itens[i].InnerXml = doc.FirstChild.InnerXml;
            }
        }

        public abstract void Order();
     
    }
}
