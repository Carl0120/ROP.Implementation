using Rop.Result;

namespace Rop.ResultExtensions
{
    public static class ResultEnsureExtensions
    {
        /// <summary>
        ///     Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
        /// </summary>
        /// <param name="resultAction"></param>
        /// <param name="predicate"></param>
        /// <param name="errorValidation"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ResultAction<T> Ensure<T>(
            this ResultAction<T> resultAction,
            Func<T, bool> predicate,
            ErrorValidation errorValidation)
        {
            if (!resultAction.IsSuccess)
                return resultAction;

            return predicate(resultAction.Value!) ? resultAction : ResultAction<T>.BadRequest(errorValidation);
        }

        /// <summary>
        ///     Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
        /// </summary>
        /// <param name="resultAction"></param>
        /// <param name="predicate"></param>
        /// <param name="errorId"></param>
        /// <param name="errorValidation"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ResultAction<T> Ensure<T>(
            this ResultAction<T> resultAction,
            Func<T, bool> predicate,
            string errorId,
            string errorValidation)
        {
            if (!resultAction.IsSuccess)
                return resultAction;

            return predicate(resultAction.Value!)
                ? resultAction
                : ResultAction<T>.BadRequest(new ErrorValidation(errorId, errorValidation));
        }

        /// <summary>
        ///     Asegurarse que el tipo que contiene cumple cierta condicion
        /// </summary>
        /// <param name="value"></param>
        /// <param name="predicate"></param>
        /// <param name="errorId"></param>
        /// <param name="errorDescription"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static ResultAction<T> Ensure<T>(
            this T? value,
            Func<T, bool> predicate,
            string errorId,
            string errorDescription)
        {
            if (value == null)
                return ResultAction<T>.BadRequest(new ErrorValidation(errorId, errorDescription));

            if (predicate(value))
                ResultAction<T>.Success(value);

            return ResultAction<T>.BadRequest(new ErrorValidation(errorId, errorDescription));
        }

        /// <summary>
        ///     Asegurarse que el tipo que contiene cumple con un grupo de condiciones
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validators"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static ResultAction<T> Ensure<T>(
            this T value,
            params (Func<T, bool> predicate, string errorId, string errorDescription)[] validators)
        {
            return Combine(validators.Select(validator => Ensure(value, validator.predicate, validator.errorId, validator.errorDescription)).ToArray());
        }

        /// <summary>
        ///     Combina varios ResultActions con tipo en uno
        /// </summary>
        /// <param name="resultActions"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static ResultAction<T> Combine<T>(params ResultAction<T>[] resultActions)
        {
            if (resultActions.All(e => e.IsSuccess))
                return ResultAction<T>.Success(resultActions.First(e => e.IsSuccess).Value!);

            var errors = resultActions
                .Where(e => !e.IsSuccess)
                .SelectMany(e =>
                    e.ValidationErrors!)
                .Distinct()
                .ToList();
            return ResultAction<T>.BadRequest(errors);
        }
    }
}
