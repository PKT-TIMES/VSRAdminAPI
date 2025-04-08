using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Model;

namespace VSRAdminAPI.Services
{
    public interface ICompanyService
    {
        public GenericResponse ValidateLogin(LoginValues loginValues);
        GenericResponse AddCompany(CustomerFileData addcustomerfromdata);
        GenericResponse LoadCompany(CompanySearch loadcompanyInput);
    }
}
