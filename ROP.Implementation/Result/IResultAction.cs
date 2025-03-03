namespace ROP.Implementation.Result;

public interface IResultAction
{
    IEnumerable<ErrorValidation> ValidationErrors { get; }
}
