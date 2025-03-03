using ROP.Implementation.Result;

namespace ROP.Implementation.ResultExtensions;

public static class ResultExtensions
{
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

    public static ResultAction<TO> Map<TI, TO>(
        this ResultAction<TI> resultAction,
        Func<TI, TO> mapper)
    {
        if (!resultAction.IsSusses)
        {
            return  new ResultAction<TO>(default, resultAction.Deconstruct());
        }
        var res = mapper(resultAction.Value!);
        return new ResultAction<TO>(res, resultAction.Deconstruct());
    }

    public static ResultAction<TO> Map<TI, TO>(
        this ResultAction<TI> resultAction,
        Func<TI, TO> mapper,
        out TI output)
    {
        if (!resultAction.IsSusses)
        {   output = default!;
            return  new ResultAction<TO>(default, resultAction.Deconstruct());
        }
        output = resultAction.Value!;
        var res = mapper(resultAction.Value!);
        return new ResultAction<TO>(res, resultAction.Deconstruct());
    }
}
