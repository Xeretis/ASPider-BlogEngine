using Domain.Data.Entities;
using Domain.Data.Repositories;
using Persistence.Common;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class FileUploadRepository : GenericRepository<FileUpload>, IFileUploadRepository
{
    public FileUploadRepository(ApplicationDbContext context) : base(context)
    {
    }
}