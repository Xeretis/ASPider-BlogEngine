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
        return await _context.FileUploads.AsNoTracking().FirstOrDefaultAsync(f => f.Filename == filename);
    }

    public async Task<FileUpload?> GetByIdWithPagePostAsync(int id)
    {
        return await _context.FileUploads
            .Include(f => f.Page)
            .Include(f => f.Post)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
}