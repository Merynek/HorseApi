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

        public void SetError(int code, string message)
        {
            this.OK = false;
            this.Error = new Error(code, message);
        }
    }

    public class Error
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public Error(int code, string message)
        {
            this.ErrorCode = code;
            this.Message = message;
        }
    }
}
