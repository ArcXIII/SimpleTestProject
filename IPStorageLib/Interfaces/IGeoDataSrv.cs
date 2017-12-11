using IPStorage.Wrappers;

namespace IPStorageLib.Interfaces
{
    internal interface IGeoDataSrv
    {
        bool Connect();
        bool Disconnect();
        GeoData GetGeoData(string ipaddress);
    }
}