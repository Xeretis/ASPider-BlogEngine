using Domain.Data.Entities;
using Domain.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;

namespace Persistence.Repositories;

public class FileUploadRepository : GenericRepository<FileUpload>, IFileUploadRepository
{
    public FileUploadRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<FileUpload?> GetByFilenameAsync(string filename)
    {
        return await _context.FileUploads.FirstOrDefaultAsync(f => f.Filename == filename);
    }
}