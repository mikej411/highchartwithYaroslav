using Newtonsoft.Json;
using System.Collections.Generic;

namespace Browser.Core.Framework
{
    /// <summary>
    /// Class that utilizes Newtonsoft Json libraries to deserialize json text to c# objects for use with linq
    /// </summary>
    public class SerializationUtils
    {
        public static List<Person> PersonDeserializer(string json)
        {
            return JsonConvert.DeserializeObject<List<Person>>(json);
        }

        public static List<RandomChartRootObject> ChartDeserializerVerifReport(string json)
        {
            return JsonConvert.DeserializeObject<List<RandomChartRootObject>>(json);
        }

        public static List<CountryChartRootObject> ChartDeserializerCountry(string json)
        {
            return JsonConvert.DeserializeObject<List<CountryChartRootObject>>(json);
        }
    }

 
}