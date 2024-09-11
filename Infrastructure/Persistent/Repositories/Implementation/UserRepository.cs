using Application.Features.Interfaces.IRepositores;
using Domain.Entities;
using Infrastructure.Persistent.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistent.Repositories.Implementation
{
    public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(QuizMasterAiDb _context, DbSet<ApplicationUser> _dbSet) : base(_context, _dbSet)
        {
        }
    }
}
