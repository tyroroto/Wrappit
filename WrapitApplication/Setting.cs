using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WrapitApplication
{
    public class Setting
    {
        public bool ResizeEnable { get; set; }
        public Size DefaultSize { get; set; }
        public string AppTitle { get; set; }
        public Size MaximumSize { get; set; }
        public Size MinimumSize { get; set; }

        private XmlDocument _doc;
        public string filename = AppDomain.CurrentDomain.BaseDirectory + "setting.config";

        public Setting()
        {
            if (File.Exists(filename))
            {
                ReadSettingFile();
            }
            else
            {
                CreateSettingFile();
            }
        }

        void ReadSettingFile()
        {
            _doc = new XmlDocument();
            _doc.Load(filename);
            try
            {
                ResizeEnable = bool.Parse(_doc.SelectSingleNode("setting/ResizeEnable").InnerText);
                var dWidth = int.Parse(_doc.SelectSingleNode("setting/DefaultWidth").InnerText);
                var dHeight = int.Parse(_doc.SelectSingleNode("setting/DefaultHeight").InnerText);
                var maxWidth = int.Parse(_doc.SelectSingleNode("setting/MaxWidth").InnerText);
                var maxHeight = int.Parse(_doc.SelectSingleNode("setting/MaxHeight").InnerText);
                var minWidth = int.Parse(_doc.SelectSingleNode("setting/MinWidth").InnerText);
                var minHeight = int.Parse(_doc.SelectSingleNode("setting/MinHeight").InnerText);
                AppTitle = _doc.SelectSingleNode("setting/AppTitle").InnerText;
                DefaultSize = new Size(dWidth,dHeight);
                MaximumSize = new Size(maxWidth, maxHeight);
                MinimumSize = new Size(minWidth, minHeight);
            }
            catch (Exception)
            {
                MessageBox.Show("Error while parse data from setting.config", "Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);

            }

        }
          
        void CreateSettingFile()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode settingNode = doc.CreateElement("setting");

            XmlNode resizeEnableNode = doc.CreateElement("ResizeEnable");
            resizeEnableNode.InnerText = "True";
            settingNode.AppendChild(resizeEnableNode);
            XmlNode defaultWidthNode = doc.CreateElement("DefaultWidth");
            defaultWidthNode.InnerText = "640";
            settingNode.AppendChild(defaultWidthNode);
            XmlNode defaultHeightNode = doc.CreateElement("DefaultHeight");
            defaultHeightNode.InnerText = "360";
            settingNode.AppendChild(defaultHeightNode);
            XmlNode maxWidthNode = doc.CreateElement("MaxWidth");
            maxWidthNode.InnerText = "640";
            settingNode.AppendChild(maxWidthNode);
            XmlNode maxHeightNode = doc.CreateElement("MaxHeight");
            maxHeightNode.InnerText = "360";
            settingNode.AppendChild(maxHeightNode);
            XmlNode minWidthNode = doc.CreateElement("MinWidth");
            minWidthNode.InnerText = "640";
            settingNode.AppendChild(minWidthNode);
            XmlNode minHeightNode = doc.CreateElement("MinHeight");
            minHeightNode.InnerText = "360";
            settingNode.AppendChild(minHeightNode);
            XmlNode appTitleNode = doc.CreateElement("AppTitle");
            appTitleNode.InnerText = "AppTitle";
            settingNode.AppendChild(appTitleNode);

            doc.AppendChild(settingNode);
            doc.Save(filename);
            ReadSettingFile();
        }

    }
}
