namespace DotNetCore.Packages.Application.Common.Services.User;

public interface IUserService
{
    Task CreteUserAsync(Domain.Entities.User user);
}