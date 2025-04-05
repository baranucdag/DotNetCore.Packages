namespace DotNetCore.Packages.Application.Common.Shared.ResultTypes;

public interface IResult
{
    bool Success { get; }
    string Message { get; }
}