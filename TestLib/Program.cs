using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IPStorageLib;
using RestSharp;

namespace TestLib
{
    class Program
    {
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        //private static IpStor _storage;
        private static RestClient _client;
        static void Main(string[] args)
        {
            if (!Connect()) return;
            var f = new StreamReader(AssemblyDirectory + @"\" + args[0]);
            string address;
            while ((address = f.ReadLine()) != null)
            {
                var request = new RestRequest("/api/ipstorage/storeip", Method.POST);
                request.AddParameter("", address);
                var resp = _client.Execute(request);
                Console.WriteLine($"{address} - {resp.Content}");
            }

            Console.ReadLine();



        }

        public static bool Connect()
        {
            try
            {
                _client = new RestClient("http://arc.bks-tv.ru:8888");
            }
            catch (Exception)
            {
                return false;
            }
            return _client != null;
        }

        public static bool Disconnect()
        {
            try
            {
                _client = null;
            }
            catch (Exception)
            {
                return false;
            }
            return _client == null;
        }
    }
}
