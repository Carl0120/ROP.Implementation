namespace ROP.Implementation.Result;

public class ResultAction<T> : IResultAction
{
    public T? Value { get; private set; }

    public string Message { get; private set; }

    public string StatusCode { get; protected set; }

    public IEnumerable<ErrorValidation> ValidationErrors { get; } = new List<ErrorValidation>();

    public bool IsSusses => !ValidationErrors.Any();

    protected ResultAction(T value, string message, string statusCode)
    {
        Value = value;
        Message = message;
        StatusCode = statusCode;
    }

    protected ResultAction(ErrorValidation error, string message, string statusCode)
    {
        Value = default;
        List<ErrorValidation> errors = new(){error};
        ValidationErrors = errors;
        StatusCode = statusCode;
        Message = message;
    }

    protected ResultAction(List<ErrorValidation> error, string message, string statusCode)
    {
        if (error.Count > 0)
        {
            StatusCode = statusCode;
            Value = default;
            ValidationErrors = error;
            Message = message;
        }
        else
        {
            throw new InvalidDataException("La lista de Errores no puede estar Vacia");
        }
    }

    //Success200
    public static ResultAction<T> Success(T value, string message = "Ok")
    {
        return new ResultAction<T>(value, message, "200");
    }

    //BadRequest400
    public static ResultAction<T> BadRequest(ErrorValidation error, string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction<T>(error, message, "400");
    }
    public static ResultAction<T> BadRequest(string identifier, string description, string message = "An ocurrido uno o mas errores de Validación")
    {
        return BadRequest(new ErrorValidation(identifier, description), message);
    }
    public static ResultAction<T> BadRequest(string message = "An ocurrido uno o mas errores de Validación")
    {
        return BadRequest(string.Empty, string.Empty, message);
    }
}
