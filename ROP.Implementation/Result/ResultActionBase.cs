namespace ROP.Implementation.Result;

public abstract class ResultActionBase
{
    public string Message { get; private set; }

    public string StatusCode { get; private set; }

    public IEnumerable<ErrorValidation> ValidationErrors { get;} = new List<ErrorValidation>();

    public bool IsSusses => !ValidationErrors.Any();

    protected ResultActionBase(string message, string statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }

    protected ResultActionBase(ErrorValidation error, string message, string statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        ValidationErrors = new List<ErrorValidation> { error };
    }

    protected ResultActionBase(IEnumerable<ErrorValidation> error, string message, string statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        ValidationErrors = error;
    }

}
