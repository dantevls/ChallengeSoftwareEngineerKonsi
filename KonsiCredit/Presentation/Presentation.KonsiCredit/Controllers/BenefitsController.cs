using Domain.KonsiCredit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.KonsiCredit.BenefitsAppService;

namespace Presentation.KonsiCredit.Controllers;

[Route("konsiCredit/v1")]
public class BenefitsController: ControllerBase
{
    
    private readonly ILogger<User> _logger;
    private readonly IConfiguration _configuration;
    private readonly IBenefitsAppService _benefitsAppService;

    public BenefitsController(ILogger<User> logger, IConfiguration configuration,
        IBenefitsAppService benefitsAppService)
    {
        _configuration = configuration;
        _logger = logger;
        _benefitsAppService = benefitsAppService;
    }
    
    [HttpGet]
    [Route("consulta-beneficios")]
    public async Task<IActionResult> GetByCpf([FromQuery] string cpf)
    {
        var response = await _benefitsAppService.GetUserBenefits(cpf);
        return Ok(response);
    }

}