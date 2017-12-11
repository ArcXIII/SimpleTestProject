using Newtonsoft.Json;

namespace IPStorage.Wrappers
{
    public class GeoData
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_name")]
        public string RegionName { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("zip_code")]
        public int? ZipCode { get; set; }

        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }

        [JsonProperty("latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal Longitude { get; set; }

        [JsonProperty("metro_code")]
        public int MetroCode { get; set; }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public override string ToString()
        {
            var props = GetType().GetProperties();
            string result = string.Empty;
            foreach (var prop in props)
            {
                result += prop.Name + ": " + GetPropValue(this, prop.Name) + "\r\n";
            }
            return result;
        }
    }
}
