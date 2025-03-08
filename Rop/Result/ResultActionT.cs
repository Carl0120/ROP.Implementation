namespace Rop.Result
{
    public class ResultAction<T> : ResultActionBase
    {
        internal ResultAction(T? value, ( IEnumerable<ErrorValidation>? errors, string Message, ResultCode StatusCode) data)
            : base(data.errors, data.Message, data.StatusCode)
        {
            Value = value;
            ValidateStatus();
        }

        private ResultAction(T value, string message, ResultCode statusCode) : base(message, statusCode)
        {
            Value = value;
            ValidateStatus();
        }

        private ResultAction(ErrorValidation error, string message, ResultCode statusCode) : base(error, message,
            statusCode)
        {
            Value = default;
            ValidateStatus();
        }

        private ResultAction(IEnumerable<ErrorValidation> error, string message, ResultCode statusCode) : base(error,
            message, statusCode)
        {
            Value = default;
            ValidateStatus();
        }

        public T? Value { get; }

        public override bool IsSuccess => base.IsSuccess && Value is not null;

        internal (IEnumerable<ErrorValidation>? validationErrors, string message, ResultCode statusCode) Deconstruct()
        {
            return new ValueTuple<IEnumerable<ErrorValidation>?, string, ResultCode>
            (
                ValidationErrors,
                Message,
                StatusCode
            );
        }

        private void ValidateStatus()
        {
            if (Value is null && ValidationErrors is null)
                throw new ArgumentNullException(nameof(ResultAction<T>), "Debe proporcionar un valor o un error");
        }

        //Success200
        public static ResultAction<T> Success(T value, string message = "Ok")
        {
            if (value is null)
                return new ResultAction<T>(new[] { new ErrorValidation("InvalidData", "Dato Invalido") },
                    "No se encontro en recurso solicitado", ResultCode.NotFound);

            return new ResultAction<T>(value, message, ResultCode.Ok);
        }

        //BadRequest400
        public static ResultAction<T> BadRequest(ErrorValidation error,
            string message = "An ocurrido uno o mas errores de Validación")
        {
            return new ResultAction<T>(error, message, ResultCode.BadRequest);
        }

        public static ResultAction<T> BadRequest(IEnumerable<ErrorValidation> error,
            string message = "An ocurrido uno o mas errores de Validación")
        {
            return new ResultAction<T>(error.ToList(), message, ResultCode.BadRequest);
        }

        public static ResultAction<T> BadRequest(string identifier, string description,
            string message = "An ocurrido uno o mas errores de Validación")
        {
            return BadRequest(new ErrorValidation(identifier, description), message);
        }

        //NotFound404
        public static ResultAction<T> NotFound(string resource = "solicitado")
        {
            return new ResultAction<T>(ErrorValidation.Empty(), $"No se encontro en recurso {resource}",
                ResultCode.NotFound);
        }

        //Unauthorized401
        public static ResultAction<T> Unauthorized(string resource = "solicitado")
        {
            return new ResultAction<T>(ErrorValidation.Empty(), $"No esta autorizado para acceder al recurso {resource}",
                ResultCode.Unauthorized);
        }
    }
}
