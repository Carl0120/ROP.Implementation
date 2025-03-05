namespace ROP.Implementation.Result;

public class ResultAction<T> : ResultActionBase
{
    public T? Value { get; private set; }

    internal ResultAction(T? value, ( IEnumerable<ErrorValidation> errors,string Message, string StatusCode) data) : base(data.errors.ToList(),data.Message,data.StatusCode)
    {
        if (value is null && !data.errors.Any())
            throw new InvalidDataException("La lista de Errores no puede estar Vacia");
        Value = value;
    }

    private ResultAction(T value, string message, string statusCode) : base(message, statusCode) { Value = value; }

    private ResultAction(ErrorValidation error, string message, string statusCode) : base(error, message, statusCode) { Value = default; }

    private ResultAction(IEnumerable<ErrorValidation> error, string message, string statusCode) : base(error, message, statusCode)
    {
        if (!error.Any())
            throw new InvalidDataException("La lista de Errores no puede estar Vacia");
        Value = default;

    }

    internal (IEnumerable<ErrorValidation> validationErrors,  string message,  string statusCode) Deconstruct()
    {
        return new()
        {
            validationErrors = ValidationErrors,
            message = Message,
            statusCode = StatusCode
        };
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
    public static ResultAction<T> BadRequest(IEnumerable<ErrorValidation> error, string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction<T>(error.ToList(), message, "400");
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
