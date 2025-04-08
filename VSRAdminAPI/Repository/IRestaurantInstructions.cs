using VSRAdminAPI.Model;

namespace VSRAdminAPI.Repository
{
    public interface IRestaurantInstructions
    {
        public OPRes AddInstruction(ReqInput reqInput);
        public List<LoadInstructionRes> LoadInstruction(int customerid);
    }
}
