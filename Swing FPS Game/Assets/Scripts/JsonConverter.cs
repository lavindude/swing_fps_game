using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System;
//using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class JsonConverter
{
    public static string Serialize<T>(T item)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            new DataContractJsonSerializer(typeof(T)).WriteObject(ms, item);
            return Encoding.Default.GetString(ms.ToArray());
        }
    }

    public static T Deserialize<T>(string body)
    {
        using (var stream = new MemoryStream())
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(stream);
        }
    }
}
