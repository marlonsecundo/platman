using System;
using System.Collections.Generic;
using System.Xml;
using Tilemap_Converter.Base;
using Tilemap_Converter.Screen;

namespace Tilemap_Converter
{
    public class Screenn : Tilemap
    {
        List<Layer> layers;

        public override XmlDocument Convert(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);
            document.LoadXml(xml);

            {
                RenameLayers(ref document);

                RemoveTags(ref document);

                Bounds(ref document);
            }

            layers = new List<Layer>();
            layers.Add(new Art());
            layers.Add(new Button());
            layers.Add(new Textt());
            layers.Add(new Comp());

            for (int i = 0; i < layers.Count; i++)
                layers[i].Convert(ref document);

            document.FirstChild.InnerXml += "<path>Textures/Screen/</path>";

            Order(ref document);

            return document;
        }

        private void Order(ref XmlDocument document)
        {
            for (int i = 0; i < document.FirstChild.ChildNodes.Count; i++)
            {
                XmlNode path = document.GetElementsByTagName("path")[0];
                XmlNode arts = document.GetElementsByTagName("arts")[0];
                XmlNode buttons = document.GetElementsByTagName("buttons")[0];
                XmlNode texts = document.GetElementsByTagName("texts")[0];
                XmlNode comps = document.GetElementsByTagName("comps")[0];
                XmlNode bounds = document.GetElementsByTagName("bounds")[0];

                string xml = "<XnaContent><Asset Type=\"Model.Screen.ScreenModel\">";

                xml += path.OuterXml + arts.OuterXml + buttons.OuterXml + texts.OuterXml + comps.OuterXml + bounds.OuterXml;

                xml += "</Asset></XnaContent>";

                document.LoadXml(xml);
            }
        }
    }
}
