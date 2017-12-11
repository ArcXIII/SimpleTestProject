using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace IPStorageLib.Controllers
{
    internal class DbController : IDisposable
    {
        private MySqlConnection _connection;
        private string _database;
        private string _password;
        private string _server;
        private string _uid;

        public DbController() : this("localhost", "ipstorage", "root", "toor")
        {
        }

        protected DbController(string srv, string db, string uid, string pass)
        {
            Init(srv, db, uid, pass);
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open) CloseConnection();
            _connection?.Dispose();
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:

                        break;

                    case 1045:

                        break;
                }
                return false;
            }
        }

        //Close connection
        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool CloseConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        private void Init(string srv, string db, string uid, string pass)
        {
            _server = srv;
            _database = db;
            _uid = uid;
            _password = pass;

            var connectionString = "SERVER=" + _server + ";" + "DATABASE=" +
                                   _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";";

            _connection = new MySqlConnection(connectionString);
        }

        public int StoreQuery(string query)
        {
            if (query == null) return -1;
            if (!OpenConnection()) return -1;

            int res;

            using (var cmd = new MySqlCommand("uquery_addorget", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ipordns", query);
                res = (int)cmd.ExecuteScalar();
            }

            CloseConnection();

            return res;
        }

        public Dictionary<string, string> GetStatByIp()
        {
            if (!OpenConnection()) return new Dictionary<string, string>();

            var res = new Dictionary<string, string>();

            using (var cmd = new MySqlCommand("get_stat_byip", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                res.Add("IpCount", cmd.ExecuteScalar().ToString());
            }
            return res;
        }

        public Dictionary<string, string> GetStatByCountry(string country = "")
        {
            if (!OpenConnection()) return new Dictionary<string, string>();

            var res = new Dictionary<string, string>();

            using (var da = new MySqlDataAdapter())
            {
                using (var cmd = new MySqlCommand("get_stat_bycountry", _connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@country", country);
                    da.SelectCommand = cmd;

                    var ds = new DataSet();
                    da.Fill(ds, "res");

                    var dt = ds.Tables["res"];

                    foreach (DataRow dr in dt.Rows)
                        res.Add(dr["Country"].ToString(), dr["IpCount"].ToString());
                    //var cntry = new JProperty("Country", dr["Country"]);
                    //var count = new JProperty("IpCount", dr["IpCount"]);
                    //var row = new JObject(cntry, count);
                    //a.Add(row);
                }
            }

            //var result = new JObject(
            //    new JProperty("result", a)
            //    );

            CloseConnection();

            return res;
        }

        public int AddOrUpdateGeo(int qid, string ip = "", string countrycode = "", string countryname = "",
            string regioncode = "", string regionname = "", string city = "", int zip = 0, string timezone = "",
            decimal lat = 0, decimal longt = 0, int metrocode = 0)
        {
            if (!OpenConnection()) return -1;

            int res;

            using (var cmd = new MySqlCommand("geodata_addorupdate", _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@uq_id", qid);
                cmd.Parameters.AddWithValue("@varIP", ip);
                cmd.Parameters.AddWithValue("@varCountryCode", countrycode);
                cmd.Parameters.AddWithValue("@varCountryName", countryname);
                cmd.Parameters.AddWithValue("@varRegionCode", regioncode);
                cmd.Parameters.AddWithValue("@varRegionName", regionname);
                cmd.Parameters.AddWithValue("@varCity", city);
                cmd.Parameters.AddWithValue("@varZipCode", zip);
                cmd.Parameters.AddWithValue("@varTimeZone", timezone);
                cmd.Parameters.AddWithValue("@varLatitude", lat);
                cmd.Parameters.AddWithValue("@varLongtitude", longt);
                cmd.Parameters.AddWithValue("@varMetroCode", metrocode);
                res = (int)cmd.ExecuteScalar();
            }

            CloseConnection();

            return res;
        }
    }
}