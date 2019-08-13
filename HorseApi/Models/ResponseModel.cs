using HorseApi.Enums;

namespace HorseApi.Models
{
    public class ResponseModel
    {
        public bool OK { get; set; }
        public object ResponseData { get; set; }
        public Error Error { get; set; }

        public void SetResponseData(object data)
        {
            this.OK = true;
            this.ResponseData = data;
        }

        public void SetError(ReponseErrorType errorType)
        {
            this.OK = false;
            this.Error = new Error(errorType);
        }
    }

    public class Error
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public Error(ReponseErrorType errorType)
        {
            this.ErrorCode = (int)errorType;
            this.Message = getErrorMessage(errorType);
        }

        private string getErrorMessage(ReponseErrorType errorType)
        {
            switch (errorType)
            {
                case ReponseErrorType.USER_NOT_FOUND: return "User not found";
                case ReponseErrorType.INVALID_PASSWORD: return "Invalid password";
                case ReponseErrorType.INVALID_REFRESH_TOKEN: return "Invalid refresh token";
                default: return "Unknown error";
            }
        }
    }
}
