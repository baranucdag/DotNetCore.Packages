using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Packages.WebApi.Controllers;

public class AuthControllers : BaseController
{
    [HttpGet("GetDefault")]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
}