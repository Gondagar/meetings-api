using MeetingsDomain.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace MeetingsAPI.Services
{
    public class AvatarService : IAvatarService
    {
        private IFileService _fileService;
        private readonly string _avatarDirectoryPath = @"\Upload\Avatar\";

        public AvatarService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public string SaveAvatar(IFormFile formFile)
        {
            if(formFile == null)
            {
                return null;
            }

            var distFile = MakeSquarePhoto(formFile);
            var fileName = Guid.NewGuid().ToString();

            return _fileService.SaveFile(fileName, _avatarDirectoryPath, distFile);
        }

        public Image MakeSquarePhoto(IFormFile file)
        {
            var imageDimension = 40;

            using (var image = Image.FromStream(file.OpenReadStream()))
            {
                var destRect = new Rectangle(0, 0, imageDimension, imageDimension);
                var destImage = new Bitmap(imageDimension, imageDimension);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
        }

        public string GetBase64Avatar(string fileName)
        {
            if(fileName == null || fileName == "")
            {
                return "";
            }

            return _fileService.GetBase64Image(fileName);
        }
    }
}
