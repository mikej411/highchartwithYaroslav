using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Browser.Core.Framework
{
    public static class WebApiServices
    {

        //Data Retrieval
        //TODO: make generic and compress into one request builder
        public static List<Person> GetPersonData(string age, string name)
        {
            var webapiurl = WebApiServices.APIRequestBuilder(age, name);

            var client = new RestClient(webapiurl);
            var json = client.MakeRequest();
            return SerializationUtils.PersonDeserializer(json);
        }


        #region Request Handlers

        //combine RestClient pieces with request building with optional performance parameters
        public static List<T> GetDelivery<T>(string webapiurl, bool performance = false)
        {
            var client = new RestClient(webapiurl);
            var json = client.MakeRequest();
            var delivery = SerializationService.DeserializeJson<T>(json);
            return delivery;
        }

        #endregion

        #region Request Builders
        public static string APIRequestBuilder(string age, string name)
        {
            return APIRequestBuilder(EnvironmentBuilder(), RequestHelper(age, name));
        }

        public static string EnvironmentBuilder(bool actuals = false, bool programLevel = false)
        {
            string environment = "";

            environment += ConfigurationManager.AppSettings["APIUrl"];

            return environment;
        }

        public static string APIRequestBuilder(string environment, NameValueCollection uriData = null)
        {
            string uri = environment;

            if (uriData != null)
            {
                for (int i = 0; i < uriData.Count; i++)
                {
                    uri += uriData.GetKey(i) + uriData.GetValues(i)[0];
                    uri += "&";
                }
            }

            if(uriData == null)
                return uri;
            else
                return uri.Substring(0, uri.Length - 1);
        }

        #endregion

        #region Name Value Pair Builder
        //Opted to use overloads over multiple optional parameters
        //Actuals
        public static NameValueCollection RequestHelper(string age, string name, bool missingAge = false, bool missingName = false)
        {
            NameValueCollection APInvc = new NameValueCollection();
            //Network
            if(!missingAge)
                APInvc.Add("age=", age);
            
            //Dates
            if(!missingName)
                APInvc.Add("name=", name);

            //Demos

            return APInvc;
        }
   
        #endregion

        #region Builder Helpers
        
        /// <summary>
        /// Helper to add http commas into request
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CommaList(List<string> input)
        {
            string toBeReturned = "";

            foreach (string str in input)
            {
                toBeReturned += str + @"%2c";
            }

            //Return with removed end comma
            return toBeReturned.Substring(0, toBeReturned.Length - 3);
        }

        #endregion
    }
}