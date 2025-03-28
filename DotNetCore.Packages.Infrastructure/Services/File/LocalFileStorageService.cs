
using DotNetCore.Packages.Application.Common.Services.File;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.Packages.Infrastructure.Services.File;

public class LocalFileStorageService : IFileStorageService
{
    public Task<string> UploadFileAsync(IFormFile file, string containerName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Stream?> GetFileAsync(string fileName, string containerName)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteFileAsync(string fileName, string containerName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}