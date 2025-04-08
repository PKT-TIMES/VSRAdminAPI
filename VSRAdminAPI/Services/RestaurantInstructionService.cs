using System.Dynamic;
using VSRAdminAPI.Model.Common;
using VSRAdminAPI.Model;
using VSRAdminAPI.Repository;

namespace VSRAdminAPI.Services
{
    
    public class RestaurantInstructionService : IRestaurantInstructionService
    {
        private readonly IRestaurantInstructions restaurantInstructions;
        public RestaurantInstructionService (IRestaurantInstructions _restaurantInstructions)
        {
            restaurantInstructions = _restaurantInstructions;
        }

        public GenericResponse AddInstruction(ReqInput reqInput)
        {
            dynamic dynamic = new ExpandoObject();
            GenericResponse genericResponse = new GenericResponse();
            OPRes oPRes = new OPRes();
            oPRes = restaurantInstructions.AddInstruction(reqInput);
            dynamic.status = oPRes.Status;
            dynamic.message = oPRes.Message;
            genericResponse.Data = dynamic;
            return genericResponse;
        }
        public GenericResponse LoadInstruction(int customerid)
        {
            dynamic dynamic = new ExpandoObject();
            GenericResponse genericResponse = new GenericResponse();
            List<LoadInstructionRes> orderDetails = restaurantInstructions.LoadInstruction(customerid);
            dynamic.status = 1;
            dynamic.message = "";
            dynamic.resultset = orderDetails;
            genericResponse.Data = dynamic;
            return genericResponse;
        }
    }
}
