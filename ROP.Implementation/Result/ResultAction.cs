namespace ROP.Implementation.Result;

public class ResultAction : ResultActionBase
{
    internal ResultAction(( IEnumerable<ErrorValidation> errors,string Message, string StatusCode) data) : base(data.errors.ToList(),data.Message,data.StatusCode) { }

    internal (IEnumerable<ErrorValidation> validationErrors,  string message,  string statusCode) Deconstruct()
    {
        return new()
        {
            validationErrors = ValidationErrors,
            message = Message,
            statusCode = StatusCode
        };
    }

    private ResultAction(string message, string statusCode) : base(message, statusCode){}

    private ResultAction(List<ErrorValidation> error, string message, string statusCode) : base(error, message, statusCode){ }

    private ResultAction(ErrorValidation error, string message, string statusCode) : base(error, message, statusCode){}

    //Success200
    public static ResultAction Success(string successMessage = "OK")
    {
        return new ResultAction(successMessage, "200");
    }

    //BadRequest400
    public static ResultAction BadRequest(ErrorValidation error,string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction(error, message, "400");
    }
    public static ResultAction BadRequest(string identifier, string description, string message = "An ocurrido uno o mas errores de Validación")
    {
        return  BadRequest(new ErrorValidation(identifier, description), message);
    }
    public static ResultAction BadRequest(string message = "An ocurrido uno o mas errores de Validación")
    {
        return  BadRequest(string.Empty, string.Empty, message);
    }
}
