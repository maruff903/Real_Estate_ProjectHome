namespace RealEstateHub.Application.Interfaces.Services;

public interface IFileService
{
    Task<string> SaveImageAsync(Stream image, string fileName, CancellationToken cancellationToken = default);
}
