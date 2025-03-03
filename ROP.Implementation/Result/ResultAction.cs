namespace ROP.Implementation.Result;

public class ResultAction : ResultAction<Unit>
{
    private ResultAction(Unit unit,string message, string statusCode) : base(unit, message, statusCode)
    {
    }

    private ResultAction(ErrorValidation error, string message, string statusCode) : base(error, message,statusCode)
    {
    }

    protected ResultAction(List<ErrorValidation> error, string message, string statusCode) : base(error, message, statusCode)
    {
    }

    //Success200
    public static ResultAction Success(string successMessage = "OK")
    {
        return new ResultAction(new Unit(), successMessage, "200");
    }

    //BadRequest400
    public new static ResultAction BadRequest(ErrorValidation error,string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction(error,message, "400");
    }
    public new static ResultAction BadRequest(string identifier, string description, string message = "An ocurrido uno o mas errores de Validación")
    {
        return  BadRequest(new ErrorValidation(identifier, description), message);
    }
    public new static ResultAction BadRequest(string message = "An ocurrido uno o mas errores de Validación")
    {
        return  BadRequest(string.Empty, string.Empty, message);
    }
}
