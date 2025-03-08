using Rop.Result;

namespace Rop.ResultExtensions
{
    public static class ResultExtensions
    {
        public static ResultAction<TO> Bind<TO, TI>(
            this ResultAction<TI> resultAction,
            Func<TI, ResultAction<TO>> predicate)
        {
            return resultAction.IsSuccess
                ? predicate(resultAction.Value!)
                : new ResultAction<TO>(default, resultAction.Deconstruct());
        }

        public static async Task<ResultAction<TO>> Bind<TO, TI>(
            this ResultAction<TI> resultAction,
            Func<TI, Task<ResultAction<TO>>> predicate)
        {
            return resultAction.IsSuccess
                ? await predicate(resultAction.Value!)
                : new ResultAction<TO>(default, resultAction.Deconstruct());
        }

        public static Result.ResultAction Bind<TI>(
            this ResultAction<TI> resultAction,
            Func<TI, Result.ResultAction> predicate)
        {
            return resultAction.IsSuccess
                ? predicate(resultAction.Value!)
                : new Result.ResultAction(resultAction.Deconstruct());
        }

        public static async Task<Result.ResultAction> Bind<TI>(
            this ResultAction<TI> resultAction,
            Func<TI, Task<Result.ResultAction>> predicate)
        {
            return resultAction.IsSuccess
                ? await predicate(resultAction.Value!)
                : new Result.ResultAction(resultAction.Deconstruct());
        }

        public static ResultAction<TO> Bind<TO, TI>(
            this ResultAction<TI> resultAction,
            Func<TI, ResultAction<TO>> predicate
            , out TI? value)
        {
            if (!resultAction.IsSuccess)
            {
                value = default;
                return new ResultAction<TO>(default, resultAction.Deconstruct());
            }

            value = resultAction.Value;
            return predicate(value!);
        }

        public static Result.ResultAction Bind<TI>(
            this ResultAction<TI> resultAction,
            Func<TI, Result.ResultAction> predicate
            , out TI? value)
        {
            if (!resultAction.IsSuccess)
            {
                value = default;
                return new Result.ResultAction(resultAction.Deconstruct());
            }

            value = resultAction.Value;
            return predicate(value!);
        }

        public static ResultAction<TI> Tap<TI>(
            this ResultAction<TI> resultAction,
            Action<TI> predicate)
        {
            if (!resultAction.IsSuccess)
                return resultAction;

            predicate(resultAction.Value!);
            return resultAction;
        }

        public static async Task<ResultAction<TI>> Tap<TI>(
            this ResultAction<TI> resultAction,
            Func<TI, Task> predicate)
        {
            if (!resultAction.IsSuccess)
                return resultAction;

            await predicate(resultAction.Value!);
            return resultAction;
        }
    }
}