namespace Rop.Result
{
    public class ErrorValidation
    {
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

        public string Identifier { get; }

        public string ErrorMessage { get;}

        public static List<ErrorValidation> Empty()
        {
            return new List<ErrorValidation>();
        }
    }
}
