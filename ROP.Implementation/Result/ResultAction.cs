namespace ROP.Implementation.Result;

public class ResultAction : ResultActionBase
{
    internal ResultAction(( IEnumerable<ErrorValidation>? errors, string Message, ResultCode StatusCode) data) : base(
        data.errors, data.Message, data.StatusCode)
    {
    }

    private ResultAction(string message, ResultCode statusCode) : base(message, statusCode)
    {
    }

    private ResultAction(List<ErrorValidation> error, string message, ResultCode statusCode) : base(error, message,
        statusCode)
    {
    }

    private ResultAction(ErrorValidation error, string message, ResultCode statusCode) : base(error, message,
        statusCode)
    {
    }

    internal (IEnumerable<ErrorValidation>? validationErrors, string message, ResultCode statusCode) Deconstruct()
    {
        return new ValueTuple<IEnumerable<ErrorValidation>?, string, ResultCode>
        (
            ValidationErrors,
            Message,
            StatusCode
        );
    }

    //Success200
    public static ResultAction Success(string successMessage = "OK")
    {
        return new ResultAction(successMessage, ResultCode.Ok);
    }

    //BadRequest400
    public static ResultAction BadRequest(ErrorValidation error,
        string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction(error, message, ResultCode.BadRequest);
    }

    public static ResultAction BadRequest(string identifier, string description,
        string message = "An ocurrido uno o mas errores de Validación")
    {
        return BadRequest(new ErrorValidation(identifier, description), message);
    }

    //NotFound404
    public static ResultAction NotFound(string resource = "solicitado")
    {
        return new ResultAction(ErrorValidation.Empty(), $"No se encontro en recurso {resource}", ResultCode.NotFound);
    }

    //Unauthorized401
    public static ResultAction Unauthorized(string resource = "solicitado")
    {
        return new ResultAction(ErrorValidation.Empty(), $"No esta autorizado para acceder al recurso {resource}",
            ResultCode.Unauthorized);
    }
}
