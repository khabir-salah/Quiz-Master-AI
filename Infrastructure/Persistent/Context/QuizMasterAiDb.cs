using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistent.Context
{
    public class QuizMasterAiDb : DbContext
    {
        public QuizMasterAiDb( DbContextOptions<QuizMasterAiDb> option) : base(option) { }

        public DbSet<User> User => Set<User>();
        public DbSet<Option> Option => Set<Option>();
        public DbSet<Result> Result => Set<Result>();
        public DbSet<Question> Question => Set<Question>();
        public DbSet<UserQuestionResult> UserQuestionResult => Set<UserQuestionResult>();
        public DbSet<Document> Document => Set<Document>();
        public DbSet<Assesment> Assesment => Set<Assesment>();

    }
}
