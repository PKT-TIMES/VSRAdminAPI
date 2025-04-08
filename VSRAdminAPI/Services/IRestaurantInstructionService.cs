using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Model;

namespace VSRAdminAPI.Services
{
    public interface IRestaurantInstructionService
    {
        GenericResponse AddInstruction(ReqInput reqInput);
        GenericResponse LoadInstruction(int customerid);
    }
}
