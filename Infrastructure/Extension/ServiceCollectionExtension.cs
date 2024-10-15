using Application.Features.Interfaces.IRepositores;
using Application.Features.Interfaces.IService;
using Application.Features.Queries.RetakeAssessment;
using Application.Features.Queries.Services.Implementation;
using Infrastructure.Persistent.Repositories.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void  AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IAssessmentRepository, AssessmentRepository>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        public static void AddService(this IServiceCollection services)
        {
            services.AddScoped<IAssessmentService, AssessmentService>()
                .AddScoped<IExtractQuestionService, ExtractQuestionService>()
                .AddScoped<ITextGenerator, TextGenerator>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<ITextAssessmentService, TextAssessmentService>()
                .AddScoped<ICohereService, CohereService>()
                .AddScoped<ISeedRole, SeedRole>();
        }

        public static void AddMediatRs(this IServiceCollection services)
        {
            services.AddMediatR(mR => mR.RegisterServicesFromAssemblies(typeof(RetakeAssessmentHandler).Assembly));
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
            });

        }

        
    }
}
