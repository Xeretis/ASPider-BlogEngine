using Domain.Data.Entities;

namespace Domain.Data.Repositories;

public interface IFileUploadRepository : IGenericRepository<FileUpload>
{
    Task<FileUpload?> GetByFilenameAsync(string filename);

    Task<FileUpload?> GetByIdWithPagePostAsync(int id);
}