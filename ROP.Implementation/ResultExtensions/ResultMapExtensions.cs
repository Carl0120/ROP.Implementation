using ROP.Implementation.Result;

namespace ROP.Implementation.ResultExtensions;

public static class ResultMapExtensions
{
    /// <summary>
    ///     Mappear de un tipo de valor a otro mediante una foncion de mapeo
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
        if (!resultAction.IsSuccess)
            return new ResultAction<TO>(default, resultAction.Deconstruct());

        var res = mapper(resultAction.Value!);
        return new ResultAction<TO>(res, resultAction.Deconstruct());
    }

    /// <summary>
    ///     Cambiar el tipo de un ResulAction a otro
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
        if (value == null) throw new ArgumentNullException(nameof(value));
        return !resultAction.IsSuccess
            ? new ResultAction<TO>(default, resultAction.Deconstruct())
            : new ResultAction<TO>(value, resultAction.Deconstruct());
    }

    /// <summary>
    ///     Mapear de un ResulAction con tipo a uno sin tipo
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
    ///     Agregarle un tipo a un ResulAction
    /// </summary>
    /// <param name="resultAction"></param>
    /// <param name="value"></param>
    /// <typeparam name="TO"></typeparam>
    /// <returns></returns>
    public static ResultAction<TO> Map<TO>(
        this ResultAction resultAction,
        TO value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return resultAction.IsSuccess
            ? new ResultAction<TO>(value, resultAction.Deconstruct())
            : new ResultAction<TO>(default, resultAction.Deconstruct());
    }
}