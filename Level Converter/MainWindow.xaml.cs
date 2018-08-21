using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace Tilemap_Converter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string filename;
        private void BtConverter_Click(object sender, RoutedEventArgs e)
        {
            string json = OpenFileJson();
            json = "{ \"level\":" + json + "}";

            string xml = JsonConvert.DeserializeXmlNode(json).OuterXml;


            Level level = new Level(filename);
            level.Convert(xml);

        }

        

        private void BtScreen_Click(object sender, RoutedEventArgs e)
        {
            string json = OpenFileJson();
            json = "{ \"screen\":" + json + "}";

            string xml = JsonConvert.DeserializeXmlNode(json).OuterXml;

            Screenn screen = new Screenn();
            screen.Convert(xml);
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
                    filename = split[split.Length - 1].Split('.')[0];

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
