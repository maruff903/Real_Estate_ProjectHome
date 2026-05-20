using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.ContactRequests;
using RealEstateHub.Application.Interfaces.Services;

namespace RealEstateHub.WebAPI.Controllers;

[Authorize]
public class ContactRequestsController(IContactRequestService contactRequestService) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Buyer,Admin")]
    public async Task<IActionResult> Create(CreateContactRequestDto dto, CancellationToken cancellationToken)
    {
        return FromResponse(await contactRequestService.CreateAsync(dto, UserId, cancellationToken));
    }

    [HttpGet("seller")]
    [Authorize(Roles = "Seller,Admin")]
    public async Task<IActionResult> SellerRequests(CancellationToken cancellationToken)
    {
        return FromResponse(await contactRequestService.GetForSellerAsync(UserId, cancellationToken));
    }
}
