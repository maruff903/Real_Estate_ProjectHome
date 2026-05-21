using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.DTOs.ContactRequests;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.WebAPI.Extensions;

namespace RealEstateHub.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactRequestsController(IContactRequestService contactRequestService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Buyer,Admin")]
    public async Task<IActionResult> Create(CreateContactRequestDto dto, CancellationToken cancellationToken)
    {
        return this.FromResponse(await contactRequestService.CreateAsync(dto, User.GetUserId(), cancellationToken));
    }

    [HttpGet("seller")]
    [Authorize(Roles = "Seller,Admin")]
    public async Task<IActionResult> SellerRequests(CancellationToken cancellationToken)
    {
        return this.FromResponse(await contactRequestService.GetForSellerAsync(User.GetUserId(), cancellationToken));
    }
}
