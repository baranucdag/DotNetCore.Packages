using DotNetCore.Packages.Application.Common.Shared.ResultTypes;
using MediatR;

namespace DotNetCore.Packages.Application.Features.Auth.Register;

public class RegisterCommand : IRequest<IDataResult<int>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IdentityNo { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;   
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; }  = string.Empty;
}