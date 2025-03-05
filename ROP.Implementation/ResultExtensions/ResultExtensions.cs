using ROP.Implementation.Result;

namespace ROP.Implementation.ResultExtensions;

public static class ResultExtensions
{
    /// <summary>
    /// Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
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
        if (!resultAction.IsSusses)
        {
            return resultAction;
        }

        if (predicate(resultAction.Value!))
        {
            return resultAction;
        }

        return ResultAction<T>.BadRequest(errorValidation);
    }

    /// <summary>
    ///  Asegurarse que el tipo que contiene el ResultAction cumple cierta condicion
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
        if (!resultAction.IsSusses)
        {
            return resultAction;
        }

        if (predicate(resultAction.Value!))
        {
            return resultAction;
        }

        return ResultAction<T>.BadRequest(new ErrorValidation(errorId, errorValidation));
    }

    /// <summary>
    ///  Asegurarse que el tipo que contiene cumple cierta condicion
    /// </summary>
    /// <param name="value"></param>
    /// <param name="predicate"></param>
    /// <param name="errorId"></param>
    /// <param name="errorDescription"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> Ensure<T>(
        this T? value,
        Func<T, bool> predicate,
        string errorId,
        string errorDescription)
    {
        if (value == null)
        {
            return ResultAction<T>.BadRequest(new ErrorValidation(errorId, errorDescription));
        }
        if (predicate(value))
        {
            ResultAction<T>.Success(value);
        }

        return ResultAction<T>.BadRequest(new ErrorValidation(errorId, errorDescription));
    }

    /// <summary>
    ///  Asegurarse que el tipo que contiene cumple con un grupo de condiciones
    /// </summary>
    /// <param name="value"></param>
    /// <param name="validators"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> Ensure<T>(
        this T value,
        params (Func<T, bool> predicate, string errorId, string errorDescription)[] validators)
    {
        List<ResultAction<T>> resultActions = new List<ResultAction<T>>();
        foreach (var validator in validators)
        {
            resultActions.Add(Ensure(value, validator.predicate, validator.errorId, validator.errorDescription));
        }

        return Combine(resultActions.ToArray());
    }

    /// <summary>
    /// Combina varios ResultActions con tipo en uno
    /// </summary>
    /// <param name="resultActions"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> Combine<T>(params ResultAction<T>[] resultActions)
    {
        if (resultActions.Any(e => !e.IsSusses))
        {
            var errors = resultActions
                .Where(e => !e.IsSusses)
                .SelectMany(e =>
                    e.ValidationErrors)
                .Distinct()
                .ToList();
            return ResultAction<T>.BadRequest(errors);
        }

        return ResultAction<T>.Success(resultActions.First(e => e.IsSusses).Value!);
    }

    /// <summary>
    /// Mappear de un tipo de valor a otro mediante una foncion de mapeo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Map<TI, TO>(
        this ResultAction<TI> resultAction,
        Func<TI, TO> mapper)
    {
        if (!resultAction.IsSusses)
        {
            return new ResultAction<TO>(default, resultAction.Deconstruct());
        }

        var res = mapper(resultAction.Value!);
        return new ResultAction<TO>(res, resultAction.Deconstruct());
    }

    /// <summary>
    /// Cambiar el tipo de un ResulAction a otro
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TI"></typeparam>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Map<TI, TO>(
        this ResultAction<TI> resultAction,
        TO value)
    {
        if (!resultAction.IsSusses || value is null)
        {
            return new ResultAction<TO>(default, resultAction.Deconstruct());
        }

        return new ResultAction<TO>(value, resultAction.Deconstruct());
    }

    /// <summary>
    /// Mapear de un ResulAction con tipo a uno sin tipo
    /// </summary>
    /// <param name="resultAction"></param>
    /// <typeparam name="TI"></typeparam>
    /// <returns></returns>
    public static ResultAction Map<TI>(
        this ResultAction<TI> resultAction)
    {
        return new ResultAction(resultAction.Deconstruct());
    }

    /// <summary>
    /// Agregarle un tipo a un ResulAction
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Map<TO>(
        this ResultAction resultAction,
        TO value)
    {
        if (!resultAction.IsSusses || value == null)
        {
            return new ResultAction<TO>(default, resultAction.Deconstruct());
        }

        return new ResultAction<TO>(value, resultAction.Deconstruct());
    }
}
