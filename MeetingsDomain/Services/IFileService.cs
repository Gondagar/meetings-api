using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Drawing;

namespace MeetingsDomain.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(string fileName, string filePath, IFormFile file);

        string SaveFile(string fileName, string filePath, Image file);

        string SaveTextFile(string fileName, string filePath, string content);

        string GetFullFilePath(string fileName, string filePath);

        string GetBase64Image(string filePath);

        string GetFileContent(string filePath);
    }
}
