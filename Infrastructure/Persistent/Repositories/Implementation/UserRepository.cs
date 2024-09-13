using Application.Features.Interfaces.IRepositores;
using Domain.Entities;
using Infrastructure.Persistent.Context;


namespace Infrastructure.Persistent.Repositories.Implementation
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly QuizMasterAiDb _context;  

        public UserRepository(QuizMasterAiDb context) : base(context)
        {
            _context = context;
        }
    }
}
