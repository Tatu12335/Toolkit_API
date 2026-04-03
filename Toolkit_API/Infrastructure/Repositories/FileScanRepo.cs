using Dapper;
using Microsoft.Data.SqlClient;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Files;
using Toolkit_API.Infrastructure.Services;

namespace Toolkit_API.Infrastructure.Repositories
{
    public class FileScanRepo : IFileScanRepo
    {
        private readonly FileHasher _hasher;
        public FileScanRepo(FileHasher hasher) 
        {
            _hasher = hasher;
        }
        public async Task <Toolkit_API.Domain.Entities.Files.File>ScanFile(string filePath)
        {
            
            var hash = await _hasher.HashFileAsync(filePath);
            var fileInfo = new FileInfo(filePath);


            using (var conn = new SqlConnection())
            {
                var file = await conn.ExecuteScalarAsync<Toolkit_API.Domain.Entities.Files.File>("Insert Into ScanLog (FileName, FileHash) values (@FileName, @FileHash)", new
                {
                    FileName = fileInfo.Name,
                    FileHash = hash
                });

                return file;

            }
        }
    }
}
