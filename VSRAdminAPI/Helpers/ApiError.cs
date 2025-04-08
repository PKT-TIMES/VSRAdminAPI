namespace VSRAdminAPI.Helpers
{
    public class ApiError
    {
        public ApiError() { }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public Severity Severity { get; set; }
    }
}
