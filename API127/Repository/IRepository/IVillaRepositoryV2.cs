using API127.Models;

namespace API127.Repository.IRepository
{
    public interface IVillaRepositoryV2 : IRepository<Villa>
    {
        Task<FileUploadSummary> UploadFileAsync(Stream fileStream, string contentType);
    }
}
