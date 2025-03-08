namespace Rop.Result
{
    public abstract class ResultActionBase
    {
        protected ResultActionBase(string message, ResultCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }

        protected ResultActionBase(ErrorValidation error, string message, ResultCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
            ValidationErrors = new List<ErrorValidation> { error };
        }

        protected ResultActionBase(IEnumerable<ErrorValidation>? error, string message, ResultCode statusCode)
        {
            Message = message;
            StatusCode = statusCode;
            ValidationErrors = error;
        }

        public string Message { get; private set; }

        public ResultCode StatusCode { get; private set; }

        public IEnumerable<ErrorValidation>? ValidationErrors { get; }

        public virtual bool IsSuccess => ValidationErrors == null;
    }
}
