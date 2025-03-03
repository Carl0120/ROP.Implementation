namespace ROP.Implementation.Result;

public class ErrorValidation
{
    public string Identifier { get; set; }

    public string ErrorMessage { get; set; }

    public ErrorValidation(string errorMessage)
    {
        ErrorMessage = errorMessage;
        Identifier = string.Empty;
    }

    public ErrorValidation(string identifier, string errorMessage)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
    }
}
