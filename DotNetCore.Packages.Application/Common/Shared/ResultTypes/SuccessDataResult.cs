namespace DotNetCore.Packages.Application.Common.Shared.ResultTypes;

public class SuccessDataResult<T> : DataResult<T>
{
    public SuccessDataResult(T data, string message)
        : base(data, true, message)
    {
    }

    public SuccessDataResult(T data)
        : base(data, true)
    {
    }

    public SuccessDataResult(string message)
        : base(default, true, message)
    {
    }
}