using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CortanaExtension.Shared.Utility
{
    public static class Serializer
    {
        public static string Serialize(object obj)
        {
            DataContractJsonSerializer jsonizer = new DataContractJsonSerializer(obj.GetType());
            string jsonString = null;
            using ( MemoryStream stream = new MemoryStream() )
            {
                jsonizer.WriteObject(stream, obj);
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                jsonString = sr.ReadToEnd();
            }
            return jsonString;
        }

        public static T Deserialize<T>(string jsonString)
        {
            DataContractJsonSerializer jsonizer = new DataContractJsonSerializer(typeof(T));
            T obj;
            using (Stream stream = GenerateStreamFromString(jsonString))
            {
                obj = (T)jsonizer.ReadObject(stream);
            }
            return obj;
        }

        //public static string GetAttribute(string attributeName, string json)
        //{
        //    DataContractJsonSerializer jsonizer = new DataContractJsonSerializer(typeof(object));
        //    jsonizer.
        //}

        private static Stream GenerateStreamFromString(string s)
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
