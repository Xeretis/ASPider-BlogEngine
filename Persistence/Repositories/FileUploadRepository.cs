using Domain.Data.Entities;
using Domain.Data.Repositories;
using Persistence.Common;

namespace Persistence.Repositories;

public class FileUploadRepository : GenericRepository<FileUpload>, IFileUploadRepository
{
    public FileUploadRepository(ApplicationDbContext context) : base(context)
    {
    }
}