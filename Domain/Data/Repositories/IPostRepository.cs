using Domain.Data.Entities;

namespace Domain.Data.Repositories;

public interface IPostRepository : IGenericRepository<Post>
{
    Task<Post?> GetByIdWithFilesAsync(int id);
    Task<Post?> GetByIdWithAuthorFilesAsync(int id);
    Task<List<Post>> GetAllWithPageAuthorFiles();
    Task<List<Post>> GetFromUserWithPageAuthorFiles(string userId);
    Task<List<Post>> GetUnapprovedWithPageAuthorFiles();
}