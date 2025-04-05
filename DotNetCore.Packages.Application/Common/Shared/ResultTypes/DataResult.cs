namespace DotNetCore.Packages.Application.Common.Shared.ResultTypes;
[Serializable]
public class DataResult<T> : Result, IDataResult<T>
{
    public DataResult(T data, bool success, string message)
        : base(success, message)
    {
        Data = data;
    }

    public DataResult(T data, bool success)
        : base(success)
    {
        Data = data;
    }

    public T Data { get; set; }
}