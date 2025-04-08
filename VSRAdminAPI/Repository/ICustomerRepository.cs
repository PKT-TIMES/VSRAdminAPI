using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Model;

namespace VSRAdminAPI.Repository
{
    public interface ICustomerRepository
    {
        public List<Defaultresultset> Customerinfo(CustomerInfo addcustomerinfo, ref dynamic dbConfig);
    }
}
