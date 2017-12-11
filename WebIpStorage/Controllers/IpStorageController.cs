using System.Collections.Generic;
using System.Web.Http;
using IPStorageLib;
using IPStorageLib.Controllers;

namespace WebIpStorage.Controllers
{
    public class IpStorageController : ApiController
    {
        public static IpStor Stor = new IpStor();

        // GET api/values
        [HttpGet]
        [ActionName("getstatbyip")]
        public IDictionary<string, string> GetStatByIp()
        {
            return Stor.GetStatistics(Enums.Statistics.ByIp);
        }

        [HttpGet]
        [ActionName("getstatbycountry")]
        public Dictionary<string, string> GetStatByCountry()
        {
            return Stor.GetStatistics(Enums.Statistics.ByCountry);
        }

        [HttpGet]
        [ActionName("getstatbycountry")]
        public Dictionary<string, string> GetStatByCountry(string value)
        {
            return Stor.GetStatistics(Enums.Statistics.ByCountry, value);
        }

        // POST api/values
        [HttpPost]
        [ActionName("storeip")]
        public string StoreIp([FromBody] string value)
        {
            var result = Stor.Save(value);
            return result;
        }
    }
}