using System;
using System.IO;
using IPStorage.Wrappers;
using IPStorageLib.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace IPStorageLib.Controllers
{
    internal class FreeGeoIp : IGeoDataSrv
    {
        private static readonly JsonSerializer JSerializer = new JsonSerializer();

        public FreeGeoIp()
        {
            Connect();
        }

        private RestClient Client { get; set; }

        public bool Connect()
        {
            try
            {
                Client = new RestClient("http://localhost:8080");
            }
            catch (Exception)
            {
                return false;
            }
            return Client != null;
        }

        public bool Disconnect()
        {
            try
            {
                Client = null;
            }
            catch (Exception)
            {
                return false;
            }
            return Client == null;
        }

        public GeoData GetGeoData(string ipaddress)
        {
            PrepAddr(ref ipaddress);
            if (Client == null) throw new NotConnectedException("Could not connect to GeoIp database.");
            var request = new RestRequest($"/json/{ipaddress}");
            var resp = Client.Execute(request);
            if (resp.Content.Contains("404 page not found") || resp.Content.Equals(""))
                throw new NotFoundException("Ip geodata not found on geodata service.");
            JsonReader jr = new JsonTextReader(new StringReader(resp.Content));
            return JSerializer.Deserialize<GeoData>(jr);
        }

        private static void PrepAddr(ref string addr)
        {
            addr = addr.Replace("http://", "");
            var indx = addr.IndexOf("/");
            addr = indx <= 0 ? addr : addr.Substring(0, indx);
        }

        public void ShowGeoData(string ipaddress)
        {
            if (Client == null) throw new NotConnectedException();
            var request = new RestRequest($"/json/{ipaddress}");
            var resp = Client.Execute(request);
            JsonReader jr = new JsonTextReader(new StringReader(resp.Content));
            Console.WriteLine(JSerializer.Deserialize<GeoData>(jr).ToString());
        }
    }
}