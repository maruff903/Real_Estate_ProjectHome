using RealEstateHub.Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;

namespace RealEstateHub.Infrastructure.Services;

public class LocalFileService(IWebHostEnvironment environment) : IFileService
{
    public async Task<string> SaveImageAsync(Stream image, string fileName, CancellationToken cancellationToken = default)
    {
        var safeFileName = $"{Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant()}{Path.GetExtension(fileName)}";
        var webRoot = environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
        {
            webRoot = Path.Combine(environment.ContentRootPath, "wwwroot");
        }

        var root = Path.Combine(webRoot, "uploads");
        Directory.CreateDirectory(root);

        var path = Path.Combine(root, safeFileName);
        await using var output = File.Create(path);
        await image.CopyToAsync(output, cancellationToken);

        return $"/uploads/{safeFileName}";
    }
}
