namespace DotNetCore.Packages.Application.Common.Shared.ResultTypes;

public interface IDataResult<out T> : IResult
{
    T Data { get; }
}