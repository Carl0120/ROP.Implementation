namespace ROP.Implementation.Result;

public abstract class ResultActionBase
{
    public string Message { get; protected set; }

    public string StatusCode { get; protected set; }

    public IEnumerable<ErrorValidation> ValidationErrors { get; protected set; } = new List<ErrorValidation>();

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

    protected ResultActionBase(List<ErrorValidation> error, string message, string statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        ValidationErrors = error;
    }

}
