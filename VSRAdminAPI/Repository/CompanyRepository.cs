using System.Data;
using VSRAdminAPI.Model;
using VSRAdminAPI.Model.Common;
using Npgsql;
//using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace VSRAdminAPI.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private static string _connectionString;
        private readonly ILogger _logger;
        public IConfiguration configuration1;
        public CompanyRepository(IConfiguration configuration,ILogger<CompanyRepository> logger)
        {
            if (configuration != null)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            _logger = logger;
        }

        public List<LoginResultset> ValidateLogin(LoginValues loginValues)
        {
            List<LoginResultset> loginResultset = new List<LoginResultset>();
            //string conStr = getConnection().GetSection("connectionStrings").GetSection("DefaultConnection").Value;

            using (var npgsqlcon = new NpgsqlConnection(_connectionString))
            {
                npgsqlcon.Open();
                var npgsqlcmd = new NpgsqlCommand(
                    "SELECT * FROM public.validatelogin(@i_username, @i_password)",
                    npgsqlcon);

                npgsqlcmd.Parameters.AddWithValue("@i_username", loginValues.UserName ?? "");
                npgsqlcmd.Parameters.AddWithValue("@i_password", loginValues.Password ?? "");

                using (NpgsqlDataReader dataReader = npgsqlcmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        LoginResultset ret = new LoginResultset
                        {
                            Userid = Convert.ToInt32(dataReader["o_userid"]),
                            Password = Convert.ToInt32(dataReader["o_password"]),
                            Name = Convert.ToString(dataReader["o_name"]),
                            Mobile = Convert.ToString(dataReader["o_mobile"]),
                            Emalid = Convert.ToString(dataReader["o_emailid"]),
                            Company = Convert.ToString(dataReader["o_company"]),
                            Companyid = Convert.ToInt32(dataReader["o_companyid"]),
                            Roleid = Convert.ToInt32(dataReader["o_roleid"]),
                            Rolename = Convert.ToString(dataReader["o_rolename"]),
                            Lastlogin = Convert.ToString(dataReader["o_lastlogin"]),
                            Approvetype = Convert.ToInt32(dataReader["o_approvetype"]),
                            Status = Convert.ToInt32(dataReader["o_status"]),
                            Message = Convert.ToString(dataReader["o_message"]),
                            Issuperadmin = Convert.ToInt32(dataReader["o_issuperadmin"]),
                            Isadmin = Convert.ToInt32(dataReader["o_isadmin"]),
                            Hasapprove = Convert.ToInt32(dataReader["o_hasapprove"])
                        };
                        loginResultset.Add(ret);
                    }
                }

                npgsqlcon.Close();
            }

            return loginResultset;
        }
        public List<Defaultresultset> AddCompany(MasterCustomer company, ref dynamic dbConfig)
        {
            List<Defaultresultset> defaultresultset = new List<Defaultresultset>();

            try
            {
                //string conStr = getConnection().GetSection("connectionStrings").GetSection("DefaultConnection").Value;
                _logger.LogInformation("AddCompany: Received request: {Request}", JsonConvert.SerializeObject(company));

                using (var npgsqlcon = new NpgsqlConnection(_connectionString))
                {
                    npgsqlcon.Open();

                    const string functionName = @"
                                SELECT * 
                                FROM public.addcompanyinformation(
                                    @i_type, @i_companyid, @i_companyname, @i_customername, @i_firstname, 
                                    @i_lastname, @i_username, @i_email, @i_regnumber, @i_address1, 
                                    @i_address2, @i_address3, @i_country, @i_zipcode, @i_contactnumber, 
                                    @i_contactperson, @i_contactpersonnumber, @i_did, @i_onlineurl, 
                                    @i_deliverytype, @i_ordercomission, @i_createdby, @i_restauranttype, 
                                    @i_hours, @i_minutes, @i_queuename, @i_whatsappurl, @i_whatsappinstructions, 
                                    @i_imagepath, @i_isadmin, @i_tax, @i_taxname
                                );";

                    using (var npgsqlcmd = new NpgsqlCommand(functionName, npgsqlcon))
                    {
                        npgsqlcmd.CommandType = CommandType.Text;

                        AddQueryParameters(npgsqlcmd, company);
                        using (var dataReader = npgsqlcmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                int status = Convert.ToInt32(dataReader["o_status"]);
                                string message = Convert.ToString(dataReader["o_message"]);
                                _logger.LogInformation("Response - Status: {Status}, Message: {Message}", status, message);
                                defaultresultset.Add(new Defaultresultset { Status = status, Message = message });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string innerEx = "";
                if (ex.InnerException != null)
                {
                    innerEx += ",InnerException: " + ex.InnerException.Message;
                }
                _logger.LogError($"AddCompany: Exception for Add companny- {company}, {ex.Message},{innerEx}");
                //defaultresultset.Add(new Defaultresultset { Status = -1, Message = $"Error: {ex.Message}" });
            }
            return defaultresultset;
        }

        private void AddQueryParameters(NpgsqlCommand cmd, MasterCustomer company)
        {

            cmd.Parameters.AddWithValue("i_type", company.Type);
            cmd.Parameters.AddWithValue("i_companyid", company.CompanyId);
            cmd.Parameters.AddWithValue("i_companyname", company.CompanyName ?? "");
            cmd.Parameters.AddWithValue("i_customername", company.CustomerName ?? "");
            cmd.Parameters.AddWithValue("i_firstname", company.FirstName ?? "");
            cmd.Parameters.AddWithValue("i_lastname", company.LastName ?? "");
            cmd.Parameters.AddWithValue("i_username", company.Username ?? "");
            cmd.Parameters.AddWithValue("i_email", company.Email ?? "");
            cmd.Parameters.AddWithValue("i_regnumber", company.RegNumber ?? "");
            cmd.Parameters.AddWithValue("i_address1", company.Address1 ?? "");
            cmd.Parameters.AddWithValue("i_address2", company.Address2 ?? "");
            cmd.Parameters.AddWithValue("i_address3", company.Address3 ?? "");
            cmd.Parameters.AddWithValue("i_country", company.Country ?? "");
            cmd.Parameters.AddWithValue("i_zipcode", company.Zipcode ?? "");
            cmd.Parameters.AddWithValue("i_contactnumber", company.ContactNumber ?? "");
            cmd.Parameters.AddWithValue("i_contactperson", company.ContactPerson ?? "");
            cmd.Parameters.AddWithValue("i_contactpersonnumber", company.ContactPersonNumber ?? "");
            cmd.Parameters.AddWithValue("i_did", company.DID ?? "");
            cmd.Parameters.AddWithValue("i_onlineurl", company.OnlineURL ?? "");
            cmd.Parameters.AddWithValue("i_deliverytype", company.DeliveryType);
            cmd.Parameters.AddWithValue("i_ordercomission", company.OrderComission);
            cmd.Parameters.AddWithValue("i_createdby", company.Createdby);
            cmd.Parameters.AddWithValue("i_restauranttype", company.Restauranttype);
            cmd.Parameters.AddWithValue("i_hours", company.Hours ?? "");
            cmd.Parameters.AddWithValue("i_minutes", company.Minutes ?? "");
            cmd.Parameters.AddWithValue("i_queuename", company.Queuename ?? "");
            cmd.Parameters.AddWithValue("i_whatsappurl", company.WhatsappURL ?? "");
            cmd.Parameters.AddWithValue("i_whatsappinstructions", company.WhatsappInstructions ?? "");
            cmd.Parameters.AddWithValue("i_imagepath", company.Imagepath ?? "");
            cmd.Parameters.AddWithValue("i_isadmin", company.Isadmin);
            cmd.Parameters.AddWithValue("i_tax", company.Tax);
            cmd.Parameters.AddWithValue("i_taxname", company.Taxname ?? "");
        }

        public List<CompanyInfo> LoadCompany(CompanySearch loadcompanyInput, ref int totalrow)
        {
            string LogoFile = "";
            List<CompanyInfo> companyOutputs = new List<CompanyInfo>();
            //string connectionString = getConnection().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            //string imagepath = configuration1["ConnectionStrings : menuImageDownload"];
            // string logopath = configuration1["ConnectionStrings : restaurantlogoPath"];
            string logopath = "";
            try
            {
                _logger.LogInformation("LoadCompany: Get Company request: {Request}", JsonConvert.SerializeObject(loadcompanyInput));

                using (var npgsqlCon = new NpgsqlConnection(_connectionString))
                {
                    npgsqlCon.Open();

                    using var npgsqlCmd = new NpgsqlCommand("SELECT * FROM public.LoadCompanyInformation(@i_search, @i_pageno)", npgsqlCon);
                    npgsqlCmd.Parameters.AddWithValue("@i_search", loadcompanyInput.Search ?? string.Empty);
                    npgsqlCmd.Parameters.AddWithValue("@i_pageno", loadcompanyInput.Pageno);

                    using (NpgsqlDataReader dataReader = npgsqlCmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            CompanyInfo _companyOutput = new CompanyInfo();
                            {
                                _companyOutput.Sno = Convert.ToInt32(dataReader["o_sno"]);
                                _companyOutput.Customerid = Convert.ToInt32(dataReader["o_customerid"]);
                                _companyOutput.Customername = Convert.ToString(dataReader["o_customername"]);
                                _companyOutput.Companyname = Convert.ToString(dataReader["o_companyname"]);
                                _companyOutput.Regnumber = Convert.ToString(dataReader["o_regnumber"]);
                                _companyOutput.Address1 = Convert.ToString(dataReader["o_address1"]);
                                _companyOutput.Address2 = Convert.ToString(dataReader["o_address2"]);
                                _companyOutput.Address3 = Convert.ToString(dataReader["o_address2"]);
                                _companyOutput.Country = Convert.ToString(dataReader["o_country"]);
                                _companyOutput.Zipcode = Convert.ToString(dataReader["o_zipcode"]);
                                _companyOutput.Contactnumber = Convert.ToString(dataReader["o_contactnumber"]);
                                _companyOutput.Contactperson = Convert.ToString(dataReader["o_contactperson"]);
                                _companyOutput.Contactpernum = Convert.ToString(dataReader["o_contactpernum"]);
                                _companyOutput.Did = Convert.ToString(dataReader["o_did"]);
                                _companyOutput.Onlineurl = Convert.ToString(dataReader["o_onlineurl"]);
                                _companyOutput.Delivertype = Convert.ToInt32(dataReader["o_delivertype"]);
                                _companyOutput.Ordercomission = Convert.ToDecimal(dataReader["o_ordercomission"]);
                                _companyOutput.Hours = Convert.ToString(dataReader["o_hours"]);
                                _companyOutput.Minutes = Convert.ToString(dataReader["o_minutes"]);
                                _companyOutput.Queuename = Convert.ToString(dataReader["o_queuename"]);
                                _companyOutput.Whatsappurl = Convert.ToString(dataReader["o_whatsappurl"]);
                                _companyOutput.Whatsappinstructions = Convert.ToString(dataReader["o_whatsappinstructions"]);
                                string[] imageExtensions = { _companyOutput.Did };
                                if (Directory.Exists(logopath))
                                {
                                    var imageFiles = Directory.GetFiles(logopath)
                                                              .Where(file => imageExtensions.Contains(Path.GetFileNameWithoutExtension(file), StringComparer.OrdinalIgnoreCase))
                                                              .ToList();

                                    if (imageFiles.Count > 0)
                                    {
                                        LogoFile = Path.GetFileName(imageFiles.First());
                                        _companyOutput.Imagepath = LogoFile;
                                    }
                                    else
                                    {
                                        _companyOutput.Imagepath = "";
                                    }
                                }
                                else
                                {

                                    _companyOutput.Imagepath = "";
                                }
                                _companyOutput.Imagepath = Convert.ToString(dataReader["o_imagepath"]);
                                _companyOutput.Isadmin = Convert.ToInt16(dataReader["o_isadmin"]);
                                _companyOutput.Tax = Convert.ToDecimal(dataReader["o_tax"]);
                                _companyOutput.Taxname = Convert.ToString(dataReader["o_taxname"]);
                                totalrow = Convert.ToInt32(dataReader["o_totalrow"]);
                                companyOutputs.Add(_companyOutput);
                            }
                        }
                        npgsqlCon.Close();
                    }

                    _logger.LogInformation("LoadCompany: Response: {Response}", JsonConvert.SerializeObject(companyOutputs));
                }
            }
            catch (Exception ex)
            {
                string innerEx = "";
                if (ex.InnerException != null)
                {
                    innerEx += ",InnerException: " + ex.InnerException.Message;
                }
                _logger.LogError($"LoadCompany: Exception for Load/Get companny- {loadcompanyInput}, {ex.Message},{innerEx}");
            }

            return companyOutputs;
        }
    }
}

