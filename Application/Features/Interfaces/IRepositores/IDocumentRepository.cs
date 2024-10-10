using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Interfaces.IRepositores
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        Task<ICollection<Document>> GetAllDocumentAsync();
        Task<string?> UploadAsync(IFormFile? file);
        Task<ICollection<Question>> GetAllQuestionsAsync();
    }
}
