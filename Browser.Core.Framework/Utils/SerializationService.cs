using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Browser.Core.Framework
{
    public class SerializationService
    {
        public static List<T> DeserializeJson<T>(string json)
        {
            //try
            //{
                List<T> ToBeReturned = new List<T>();
                if (json.Substring(0, 1) != "[")
                {
                    var s = new JavaScriptSerializer();
                    ToBeReturned.Add(s.Deserialize<T>(json));
                }
                else
                {
                    var s = new JavaScriptSerializer();
                    ToBeReturned = s.Deserialize<List<T>>(json);
                }
                return ToBeReturned;
            //}
            //catch (Exception e)
            //{
            //    return null;
            //}
        }
    }
}
