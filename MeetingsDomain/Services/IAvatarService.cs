using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MeetingsDomain.Services
{
    public interface IAvatarService
    {
        string SaveAvatar(IFormFile formFile);

        string GetBase64Avatar(string fileName);
    }
}
