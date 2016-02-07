using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using static CortanaExtension.Shared.Utility.FileHelper;

namespace CortanaExtension.Shared.Utility
{
    public static class VCDEditor
    {
        public static async Task<XmlDocument> GetXMLDoc(string filename)
        {
            XmlDocument doc = new XmlDocument();
            //IStorageFile file = await FileHelper.GetFile(vcdFilename, StorageFolders.FlawlessCowboy);
            string xml = await FileHelper.ReadFrom(filename, StorageFolders.FlawlessCowboy);
            doc.LoadXml(xml);
            return doc;
        }

        public static async Task DoThing(string commandName)
        {
            string filename = Cortana.Cortana.vcdFilename;
            XmlDocument doc = await GetXMLDoc(filename);

            ////var myNewElement = new XElement("Command",
            ////   new XAttribute("Name", commandName)
            ////);

            XmlElement root = doc.DocumentElement;

            ////Create a new node.
            XmlElement elem = doc.CreateElement("price");
            elem.InnerText = "19.95";

            ////Add the node to the document.
            root.AppendChild(elem);

            string path = (await FileHelper.GetFile(filename, StorageFolders.FlawlessCowboy)).Path;
            await PopupHelper.ShowPopup("Path is: " + path);
            using (Stream s = GenerateStreamFromString(path))
            {
                doc.Save(s);
            }
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
