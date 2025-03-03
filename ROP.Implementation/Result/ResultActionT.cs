namespace ROP.Implementation.Result;

public class ResultAction<T> : IResultAction
{
    public T? Value { get; private set; }

    public string Message { get; private set; }

    public string StatusCode { get; protected set; }

    public IEnumerable<ErrorValidation> ValidationErrors { get; } = new List<ErrorValidation>();

    public bool IsSusses => !ValidationErrors.Any() && Value is not null;

    internal ResultAction(T? value, (string Message, string StatusCode, IEnumerable<ErrorValidation>) data)
    {
        if (value is null && !data.Item3.Any())
        {
            throw new InvalidDataException("La lista de Errores no puede estar Vacia");
        }

        ValidationErrors = data.Item3.ToList();
        Message = data.Message;
        StatusCode = data.StatusCode;
        Value = value;
    }

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

    internal (string Message, string StatusCode, IEnumerable<ErrorValidation>) Deconstruct()
    {
        return (Message, StatusCode, ValidationErrors);
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
