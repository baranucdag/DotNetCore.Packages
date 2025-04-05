using DotNetCore.Packages.Application.Common.Services.Auth;
using DotNetCore.Packages.Domain.Repositories;

namespace DotNetCore.Packages.Application.Common.Services.User;

public class UserService : IUserService
{
    private readonly IAuhtenticationService _auhtenticationService;
    private readonly IUserRepository _userRepository;

    public UserService(
        IAuhtenticationService auhtenticationService,
        IUserRepository userRepository)
    {
            _auhtenticationService = auhtenticationService; 
            _userRepository = userRepository;
    }

    public Task<bool> HasPermissionAsync(string permission)
    {
        return Task.FromResult(true);
    }

    public async Task CreteUserAsync(Domain.Entities.User user)
    {
       await _userRepository.AddAsync(user,default);
    }
}