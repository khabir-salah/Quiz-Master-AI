using Domain.Entities;

namespace Application.Features.Interfaces.IRepositores
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        Task<ICollection<Document>> GetAllDocumentAsync();
    }
}
