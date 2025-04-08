using System.Dynamic;
using VSRAdminAPI.Model;
using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Repository;

namespace VSRAdminAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository customerRepository;
        public CustomerService(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
            //dbConfig = _dbConfig;
        }
        public GenericResponse Customerinfo(CustomerInfo addcustomerinfo)
        {
            GenericResponse genericResponse = new GenericResponse();
            dynamic dynamic = new ExpandoObject();
            dynamic dbConfig = "";
            //List<LoadcompanyOutput> defaultresultset = new List<LoadcompanyOutput>();
            List<Defaultresultset> defaultresultset = customerRepository.Customerinfo(addcustomerinfo, ref dbConfig);
            dynamic.resultset = defaultresultset;
            genericResponse.Data = dynamic;
            return genericResponse;
        }
    }
}
