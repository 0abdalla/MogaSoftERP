using Microsoft.AspNetCore.Http;

namespace mogaERP.Domain.Interfaces.Common;
public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folderName);
}
