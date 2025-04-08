using Npgsql;
using System.Data;
using VSRAdminAPI.Model;

namespace VSRAdminAPI.Repository
{
    public class RestaurantInstructions :  IRestaurantInstructions
    {
        private static string _connectionString;
        public RestaurantInstructions(IConfiguration configuration)
        {
            if (configuration != null)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
        }
        public OPRes AddInstruction(ReqInput reqInput)
        {
            int iTotalItems = 0;
            OPRes oPRes = new OPRes();
            //string conStr = getConnection().GetSection("connectionStrings").GetSection("DefaultConnection").Value;
            using var npgsqlcon = new NpgsqlConnection(_connectionString);
            npgsqlcon.Open();
            //string Query = "INSERT INTO mastercustomer (instruction) VALUES('" + reqInput.Instruction + "') where idcustomer = '" + reqInput.Customerid + "'";
            string Query = "update mastercustomer set instructions='" + reqInput.Instruction + "' where idcustomer = '" + reqInput.Customerid + "'";
            var npgsqlcmd = new NpgsqlCommand(Query, npgsqlcon);
            npgsqlcmd.CommandType = CommandType.Text;
            npgsqlcmd.Parameters.AddWithValue("i_customerid", NpgsqlTypes.NpgsqlDbType.Integer, reqInput.Customerid);
            npgsqlcmd.Parameters.AddWithValue("i_instruction", NpgsqlTypes.NpgsqlDbType.Varchar, reqInput.Instruction);
            //using (NpgsqlDataReader dataReader = npgsqlcmd.ExecuteReader())
            iTotalItems = npgsqlcmd.ExecuteNonQuery();
            if (iTotalItems > 0)
            {
                oPRes.Status = 1;
                oPRes.Message = "Added Successfully";
            }
            else
            {
                oPRes.Status = -1;
                oPRes.Message = "Invalid data";
            }
            npgsqlcon.Close();
            return oPRes;
        }

        public List<LoadInstructionRes> LoadInstruction(int customerid)
        {
            List<LoadInstructionRes> lstOrder = new List<LoadInstructionRes>();
            //string conStr = getConnection().GetSection("connectionStrings").GetSection("DefaultConnection").Value;
            using var npgsqlcon = new NpgsqlConnection(_connectionString);
            npgsqlcon.Open();
            string Query = "select instructions from mastercustomer where idcustomer = '" + customerid + "' ";
            var npgsqlcmd = new NpgsqlCommand(Query, npgsqlcon);
            npgsqlcmd.CommandType = CommandType.Text;
            using (NpgsqlDataReader dataReader = npgsqlcmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    LoadInstructionRes currentRow = new LoadInstructionRes();
                    currentRow.Instruction = Convert.ToString(dataReader["instructions"]);
                    lstOrder.Add(currentRow);
                }
            }
            npgsqlcon.Close();
            return lstOrder;
        }
    }
}
