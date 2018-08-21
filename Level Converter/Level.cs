using Tilemap_Converter.Base;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Tilemap_Converter
{
    public class Level : Tilemap
    {
        XmlDocument doc = new XmlDocument();
        List<Layer> layers;
        string filename;
        public Level(string filename)
        {
            this.filename = filename;
        }

        public override XmlDocument Convert(string xml)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xml);

            {
                RemoveTags(ref document);

                RenameLayers(ref document);

                Bounds(ref document);
            }


            layers = new List<Layer>();
            layers.Add(new Blocks());
            layers.Add(new Death());
            layers.Add(new Text());
            layers.Add(new Poetry());
            layers.Add(new Music());
            layers.Add(new Gravity());
            layers.Add(new Final());
            layers.Add(new Hero());
            layers.Add(new Camera());
            layers.Add(new Background());
            
            for (int i = 0; i < layers.Count; i++)
                layers[i].Convert(ref document);

            Order(ref document);

            return null;
        }

        private void Order(ref XmlDocument document)
        {
            XmlNode path = document.GetElementsByTagName("path")[0];
            XmlNode blocks = document.GetElementsByTagName("blocks")[0];
            XmlNode background = document.GetElementsByTagName("background")[0];
            XmlNode deathPoints = document.GetElementsByTagName("deathPoints")[0];
            XmlNode textPoints = document.GetElementsByTagName("textPoints")[0];
            XmlNode musicPoints = document.GetElementsByTagName("musicPoints")[0];
            XmlNode poetryPoints = document.GetElementsByTagName("poetryPoints")[0];
            XmlNode gravityPoints = document.GetElementsByTagName("gravityPoints")[0];
            XmlNode finalPoints = document.GetElementsByTagName("finalPoints")[0];
            XmlNode hero = document.GetElementsByTagName("hero")[0];
            XmlNode bounds = document.GetElementsByTagName("bounds")[0];
            XmlNode camera = document.GetElementsByTagName("camera")[0];

            background.InnerXml = "<texture>space</texture>" + GetScreen();

            document.FirstChild.InnerXml = bounds.OuterXml + background.OuterXml + blocks.OuterXml + deathPoints.OuterXml + 
                textPoints.OuterXml + musicPoints.OuterXml + poetryPoints.OuterXml + gravityPoints.OuterXml 
                + finalPoints.OuterXml + hero.OuterXml + camera.OuterXml;

            AddMonoXml(ref document);
        }

        private void AddMonoXml(ref XmlDocument document)
        {
            string xml = "<XnaContent><Asset Type=\"Model.Gameplay.LevelModel\">";
            xml += document.FirstChild.InnerXml;
            xml += "</Asset></XnaContent>";

            document = new XmlDocument();
            document.LoadXml(xml);

            Util.SaveFileXml(document, @"C:\Users\Marlo\Dropbox\Platman\Project\Content\Map\", filename);
        }

        private string GetScreen()
        {
            string json = OpenFileJson();
            json = "{ \"screen\":" + json + "}";

            string xml = JsonConvert.DeserializeXmlNode(json).OuterXml;
            
            doc = new Screenn().Convert(xml);

            return "<screen>" + doc.GetElementsByTagName("Asset")[0].InnerXml + "</screen>";
        }

        private void AddBackgorundXml(XmlDocument xmlDoc)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();

            openFileDialog2.Title = "Select Json Background Map File";
            openFileDialog2.AddExtension = false;
            openFileDialog2.DefaultExt = ".json";
            openFileDialog2.Filter = "JSON Source File(*.json) | *.json";
            openFileDialog2.InitialDirectory = @"C:\Users\Marlo\Dropbox\Platman\Level Maker\Stage";
            var result = openFileDialog2.ShowDialog();

            if (result == true)
            {
                using (Stream s = openFileDialog2.OpenFile())
                {
                    using (StreamReader reader = new StreamReader(s))
                    {
                        string json = reader.ReadToEnd();
                        json = "{ \"level\":" + json + "}";
                        XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
                    }
                }
            }
            else
                throw new Exception();
        }

        public string OpenFileJson()
        {
            string dir = @"C:\Users\Marlo\Dropbox\Platman\Level Maker\Stage";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Title = "Select Json Map File";
            openFileDialog1.AddExtension = false;
            openFileDialog1.DefaultExt = ".json";
            openFileDialog1.Filter = "JSON Source File(*.json) | *.json";
            openFileDialog1.RestoreDirectory = false;
            openFileDialog1.InitialDirectory = dir;

            var result = openFileDialog1.ShowDialog();

            if (result == true)
            {
                using (Stream s = openFileDialog1.OpenFile())
                {
                    var split = openFileDialog1.FileName.Split('\\');
                    string fileName = split[split.Length - 1].Split('.')[0];
                    using (StreamReader reader = new StreamReader(s))
                    {
                        string json = reader.ReadToEnd();
                        string filePath = openFileDialog1.FileName;
                        return json;

                    }
                }
            }
            else
                throw new Exception();

        }

  

    }
}
