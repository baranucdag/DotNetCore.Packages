namespace DotNetCore.Packages.Application.Common.Services.Auth;

public interface IAuhtenticationService
{
    public bool HasPermission(string permission);
    public void EnsurePermissionForHandler<THandler>();
}