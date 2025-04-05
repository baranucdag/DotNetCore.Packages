using DotNetCore.Packages.Application.Common.Services.Cache;
using DotNetCore.Packages.Application.Common.Services.User;
using DotNetCore.Packages.Application.Common.Shared.ResultTypes;
using DotNetCore.Packages.Domain.Entities;
using MediatR;

namespace DotNetCore.Packages.Application.Features.Auth.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand,IDataResult<int>>
{
    private readonly IUserService _userService;
    private readonly IRedisCacheService _redisCacheService;
    
    public RegisterCommandHandler(
        IUserService userService,
        IRedisCacheService redisCacheService
        )
    {
        _userService = userService;
        _redisCacheService = redisCacheService;
    }
    
    public async Task<IDataResult<int>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var userToCreate = new User
        {
           Name = request.FirstName,
        };
        
       //handle operations for register
       await _userService.CreteUserAsync(userToCreate);
       
       await _redisCacheService.SetJsonAsync("user_name",data:userToCreate.Name,TimeSpan.FromMinutes(1));
       
        return new DataResult<int>(1,true);
    }
}