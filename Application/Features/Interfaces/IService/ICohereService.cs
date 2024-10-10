namespace Application.Features.Interfaces.IService
{
    public interface ICohereService
    {
        Task<string> GenerateAsync(string prompt);
    }
}
