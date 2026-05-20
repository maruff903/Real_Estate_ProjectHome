using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Interfaces.Services;

namespace RealEstateHub.WebAPI.Controllers;

[AllowAnonymous]
public class CitiesController(IReferenceDataService referenceDataService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return FromResponse(await referenceDataService.GetCitiesAsync(cancellationToken));
    }
}
