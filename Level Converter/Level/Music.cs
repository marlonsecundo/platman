﻿using System;
using System.Xml;
using Tilemap_Converter.Base;

namespace Tilemap_Converter
{
    public class Music : Layer
    {
        public Music() : base("musicPoints")
        {

        }

        public override void Convert(ref XmlDocument doc)
        {
            base.Convert(ref doc);

            Position();
            Bounds();
            Texture();
            Musicc();
            Order();
        }

        private void Musicc()
        {
            for (int i = 0; i < itens.Length; i++)
            {
                itens[i].InnerXml = itens[i].InnerXml.Replace("type", "music");
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
                XmlNode music = doc.GetElementsByTagName("music")[0];

                itens[i].InnerXml = position.OuterXml + texture.OuterXml + visible.OuterXml + bounds.OuterXml + music.OuterXml;
            }
        }
    }
}
