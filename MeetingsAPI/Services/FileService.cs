using MeetingsDomain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MeetingsAPI.Services
{
    public class FileService : IFileService
    {
        public FileService() { }

        private string GetFullPath(string filePath)
        {
            return Directory.GetCurrentDirectory() + filePath;
        }

        public string GetFullFilePath(string fileName, string filePath)
        {
            return GetFullPath(filePath) + fileName;
        }

        public string SaveTextFile(string fileName, string filePath, string content)
        {
            string fullPath = GetFullPath(filePath);
            string relativePath = filePath;
            string pathToFile = Path.Combine(fullPath, fileName);

            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            relativePath += fileName;
            File.WriteAllText(pathToFile, content);

            return relativePath;
        }

        public async Task<string> SaveFileAsync(string fileName, string filePath, IFormFile file)
        {
            string uploadPath = GetFullPath(filePath);
            string relativePath = filePath;
            string distFileName = file.FileName;

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            distFileName = distFileName.Split(".").Last();
            distFileName = Path.GetFileName(fileName + "." + distFileName);
            relativePath += distFileName;
            uploadPath += distFileName;

            using (var fileStrem = new FileStream(uploadPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStrem);
            }

            return relativePath;
        }

        public string SaveFile(string fileName, string filePath, Image file)
        {
            string uploadPath = GetFullPath(filePath);
            string relativePath = filePath;
            string distFileName = fileName + ".jpg";

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            relativePath += distFileName;
            uploadPath += distFileName;

            file.Save(uploadPath);
            return relativePath;
        }

        public string GetBase64Image(string fileName)
        {
            var fileExtension = fileName.Split(".").Last();
            string fullPath = GetFullPath(fileName);

            try
            {
                byte[] fileBytes = File.ReadAllBytes(fullPath);
                return "data:image/" + fileExtension + ";base64," + Convert.ToBase64String(fileBytes);
            }
            catch(Exception)
            {
                return null;
            }

        }

        public string GetFileContent(string filePath)
        {
            var fullPath = GetFullPath(filePath);

            if (!File.Exists(fullPath))
            {
                return "";
            }

            string readText = File.ReadAllText(fullPath);

            return readText;
        }
    }
}
