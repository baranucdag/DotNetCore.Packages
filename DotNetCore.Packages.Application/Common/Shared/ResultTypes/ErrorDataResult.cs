namespace DotNetCore.Packages.Application.Common.Shared.ResultTypes;

public class ErrorDataResult<T> : DataResult<T>
{
    public ErrorDataResult(T data, string message)
        : base(data, false, message)
    {
    }

    public ErrorDataResult(T data)
        : base(data, false)
    {
    }

    public ErrorDataResult(string message)
        : base(default, false, message)
    {
    }
}