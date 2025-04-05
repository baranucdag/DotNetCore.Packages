using DotNetCore.Packages.Application.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Packages.WebApi.Controllers;

public class AuthControllers : BaseController
{
    private readonly IMediator _mediator;

    public AuthControllers(IMediator mediator)
    {
        _mediator = mediator;   
    }
    
    [HttpGet("GetDefault")]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        await _mediator.Send(request);
        return Ok();    
    }
}