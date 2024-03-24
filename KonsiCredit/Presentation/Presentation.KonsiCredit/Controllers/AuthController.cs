using Application.KonsiCredit.AuthViewModels;
using Domain.KonsiCredit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.KonsiCredit.AuthAppService;

namespace Presentation.KonsiCredit.Controllers;

[Route("konsiCredit/Auth/v1")]
public class AuthController : ControllerBase
{
    
    private readonly ILogger<User> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAuthAppService _authAppService;

    public AuthController(ILogger<User> logger, IConfiguration configuration,
        IAuthAppService authAppService)
    {
        _configuration = configuration;
        _logger = logger;
        _authAppService = authAppService;
    }
    
    [AllowAnonymous]
    [Route("/userToken")]
    [HttpGet]
    public async Task<IActionResult> GetUserToken(UserViewModel userViewModel)
    {
        var response = await _authAppService.GetUserToken(userViewModel.Username, userViewModel.Password);
        return Ok(response);
    }
    
}