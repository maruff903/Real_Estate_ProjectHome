using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateHub.Application.Interfaces.Services;
using RealEstateHub.Application.Responses;

namespace RealEstateHub.WebAPI.Controllers;

[Authorize(Roles = "Seller,Admin")]
public class ImagesController(IFileService fileService) : ApiControllerBase
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    [HttpPost("upload")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
        {
            return FromResponse(ApiResponse<object>.Failure("File is empty."));
        }

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedContentTypes.Contains(file.ContentType) || !AllowedExtensions.Contains(extension))
        {
            return FromResponse(ApiResponse<object>.Failure("Only jpg, png, and webp image uploads are allowed.", 400));
        }

        await using var stream = file.OpenReadStream();
        var url = await fileService.SaveImageAsync(stream, file.FileName, cancellationToken);
        return FromResponse(ApiResponse<object>.Success(new { Url = url }, "Image uploaded.", 201));
    }
}
