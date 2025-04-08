using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Model;

namespace VSRAdminAPI.Services
{
    public interface ICustomerService
    {
        GenericResponse Customerinfo(CustomerInfo addcustomerinfo);
    }
}
