using VSRAdminAPI.Model;
using VSRAdminAPI.Model.Common;

namespace VSRAdminAPI.Repository
{
    public interface ICompanyRepository
    {
        public List<LoginResultset> ValidateLogin(LoginValues loginValues);
        public List<Defaultresultset> AddCompany(MasterCustomer company, ref dynamic dbConfig);
        public List<CompanyInfo> LoadCompany(CompanySearch loadcompanyInput, ref int totalrow);

    }
}
