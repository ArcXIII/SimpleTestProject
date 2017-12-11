using System;
using System.Collections.Generic;
using System.IO;
using IPStorage.Wrappers;
using RestSharp;
using Newtonsoft.Json;

namespace IPStorage
{

    class Program
    {
        public static JsonSerializer JSerializer = new JsonSerializer();
        static void Main(string[] args)
        {

            var list = new List<string> { "77.232.141.201", "google.com", "amazon.com", "no-ip.ru", "dns-shop.ru" };
            var reslit = new List<GeoData>();

            foreach (var l in list)
            {
                reslit.Add(GetGeoData(l));
            }
            foreach (var r in reslit)
            {
                Console.WriteLine(r.ToString());
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        private static GeoData GetGeoData(string ipaddr)
        {
            //var settings = new JsonSerializerSettings {FloatParseHandling = FloatParseHandling.Decimal};
            var client = new RestClient("http://localhost:8080");
            var request = new RestRequest($"/json/{ipaddr}");
            IRestResponse resp = client.Execute(request);
            JsonReader jr = new JsonTextReader(new StringReader(resp.Content));
            return JSerializer.Deserialize<GeoData>(jr);
            //Console.WriteLine(resp.Content);
            //return JsonConvert.DeserializeObject<GeoData>(resp.Content, settings);
        }
    }
}
