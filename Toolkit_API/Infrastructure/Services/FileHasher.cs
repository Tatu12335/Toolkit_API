using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace Toolkit_API.Infrastructure.Services
{
    public class FileHasher
    {
        public FileHasher() 
        {
            
        }
        public async Task<FileStream> OpenFS(string filePath)
        {
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        public async Task<byte[]> HashFileAsync(string filePath)
        {
            var sha256 = SHA256.Create();

            using (var stream = await OpenFS(filePath))
            {
                var hashBytes = sha256.ComputeHash(stream);
                return hashBytes;
                
            }
        }
    }
}
