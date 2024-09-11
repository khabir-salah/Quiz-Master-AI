using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistent.Context
{
    public class QuizMasterAiDb : IdentityDbContext<ApplicationUser>
    {
        public QuizMasterAiDb( DbContextOptions<QuizMasterAiDb> option) : base(option) { }

        public DbSet<ApplicationUser> User => Set<ApplicationUser>();
        public DbSet<Option> Option => Set<Option>();
        public DbSet<Result> Result => Set<Result>();
        public DbSet<Question> Question => Set<Question>();
        public DbSet<UserQuestionResult> UserQuestionResult => Set<UserQuestionResult>();
        public DbSet<Document> Document => Set<Document>();
        public DbSet<Assesment> Assesment => Set<Assesment>();

    }
}
