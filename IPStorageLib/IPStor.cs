using System;
using System.Collections.Generic;
using IPStorage.Wrappers;
using IPStorageLib.Controllers;
using IPStorageLib.Interfaces;

namespace IPStorageLib
{
    public class IpStor
    {

        private readonly IGeoDataSrv _geosrv;

        public IpStor(Enums.GeoDataServices gds = 0)
        {
            if (gds == Enums.GeoDataServices.FreeGeoIp)
                _geosrv = new FreeGeoIp();
        }

        private GeoData GetGeo(string ip)
        {
            var res = _geosrv.GetGeoData(ip);
            return res;
        }

        public string Save(string ipordns)
        {
            object result;
            try
            {
                result = GetGeo(ipordns);
            }
            catch (NotFoundException ex)
            {
                result = ex.Message;
            }
            catch (NotConnectedException ex)
            {
                result = ex.Message;
            }
            if (result == null || result is string) return (string)result;
            using (var dbc = new DbController())
            {
                var geo = (GeoData)result;
                var resip = dbc.StoreQuery(ipordns);
                if (resip > 0)
                    dbc.AddOrUpdateGeo(resip, geo.Ip, geo.CountryCode, geo.CountryName, geo.RegionCode,
                        geo.RegionName, geo.City, geo.ZipCode ?? 0, geo.TimeZone, geo.Latitude, geo.Longitude,
                        geo.MetroCode);
            }
            return "IP address geodata added to DB.";
        }

        public Dictionary<string, string> GetStatistics(Enums.Statistics stat, string country = "")
        {
            Dictionary<string, string> result;
            switch (stat)
            {
                case Enums.Statistics.ByIp:
                    result = GetStatByIp();
                    break;
                case Enums.Statistics.ByCountry:
                    result = GetStatByCountry(country);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
            }
            return result;
        }

        private Dictionary<string, string> GetStatByIp()
        {
            using (var dbc = new DbController())
            {
                return dbc.GetStatByIp();
            }
        }

        private Dictionary<string, string> GetStatByCountry(string country)
        {
            using (var dbc = new DbController())
            {
                return dbc.GetStatByCountry(country);
            }
        }
    }
}