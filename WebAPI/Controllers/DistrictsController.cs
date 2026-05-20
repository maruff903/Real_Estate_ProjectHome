using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Interfaces.Services;

namespace RealEstateHub.WebAPI.Controllers;

[AllowAnonymous]
public class DistrictsController(IReferenceDataService referenceDataService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid? cityId, CancellationToken cancellationToken)
    {
        return FromResponse(await referenceDataService.GetDistrictsAsync(cityId, cancellationToken));
    }
}
