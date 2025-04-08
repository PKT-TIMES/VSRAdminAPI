using System.Globalization;

namespace VSRAdminAPI.Helpers
{
    public class AppException : Exception
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public AppException() : base() { }

        public AppException(string message) : base(message)
        {
            ErrorMessage = message;
        }

        public AppException(int errorCode, string errorMessage)
            : base(errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public AppException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, message, args);
        }

        public override string ToString()
        {
            return $"ErrorCode: {ErrorCode}, ErrorMessage: {ErrorMessage}";
        }
    }
}
