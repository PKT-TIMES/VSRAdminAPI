using System.Text.Json;
using Npgsql;
using System.Data;
using VSRAdminAPI.Model;
using VSRAdminAPI.Model.Common;
using NpgsqlTypes;

namespace VSRAdminAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private static string _connectionString;
        private readonly ILogger _logger;
        public CustomerRepository(IConfiguration configuration, ILogger<CustomerRepository> logger)
        {
            if(configuration != null)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            _logger = logger;
        }

        public List<Defaultresultset> Customerinfo(CustomerInfo addcustomerinfo, ref dynamic dbConfig)
        {
            List<Defaultresultset> defaultresultset = new List<Defaultresultset>();
            //string conStr = getConnection().GetSection("connectionStrings").GetSection("DefaultConnection").Value;
            //dbConfig = getConnection().GetSection("connectionStrings");
            try
            {
                _logger.LogInformation("Customerinfo: Received request: {Request}", JsonSerializer.Serialize(addcustomerinfo));
                using (var npgsqlcon = new NpgsqlConnection(_connectionString))
                {
                    npgsqlcon.Open();
                    var npgsqlcmd = new NpgsqlCommand(
                        "SELECT * FROM public.addcustomerinfo(@i_companyid, @i_merchantid, @i_clientid, @i_secretkey, @i_secretcode, @i_authtoken, @i_pos)",
                        npgsqlcon);

                    npgsqlcmd.Parameters.AddWithValue("@i_companyid", Convert.ToInt32(addcustomerinfo.Companyid));
                    npgsqlcmd.Parameters.AddWithValue("@i_merchantid", Convert.ToString(addcustomerinfo.Merchantid ?? ""));
                    npgsqlcmd.Parameters.AddWithValue("@i_clientid", Convert.ToString(addcustomerinfo.Clientid ?? ""));
                    npgsqlcmd.Parameters.AddWithValue("@i_secretkey", Convert.ToString(addcustomerinfo.Secretkey ?? ""));
                    npgsqlcmd.Parameters.AddWithValue("@i_secretcode", Convert.ToString(addcustomerinfo.Secretcode ?? ""));
                    npgsqlcmd.Parameters.AddWithValue("@i_authtoken", Convert.ToString(addcustomerinfo.Authtoken ?? ""));
                    npgsqlcmd.Parameters.AddWithValue("@i_pos", Convert.ToString(addcustomerinfo.Pos ?? ""));

                    using (NpgsqlDataReader dataReader = npgsqlcmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Defaultresultset _defaultresult = new Defaultresultset
                            {
                                Status = Convert.ToInt32(dataReader["o_status"]),
                                Message = Convert.ToString(dataReader["o_message"])
                            };
                            defaultresultset.Add(_defaultresult);
                        }
                    }
                    _logger.LogInformation("Customerinfo: Response: {Response}", JsonSerializer.Serialize(defaultresultset));
                    npgsqlcon.Close();
                }
            }
            catch (Exception ex)
            {
                string innerEx = "";
                if (ex.InnerException != null)
                {
                    innerEx += ",InnerException: " + ex.InnerException.Message;
                }
                _logger.LogError($"Customerinfo: Exception for Add Customerinfo- {addcustomerinfo}, {ex.Message},{innerEx}");
            }
            
            return defaultresultset;
        }

    }
}
