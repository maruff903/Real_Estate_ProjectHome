using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.WebAPI.Extensions;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class DistrictsController(IReferenceDataService referenceDataService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int? cityId, CancellationToken cancellationToken)
    {
        return this.FromResponse(await referenceDataService.GetDistrictsAsync(cityId, cancellationToken));
    }
}
