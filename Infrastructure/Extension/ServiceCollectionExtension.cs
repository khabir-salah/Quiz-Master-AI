using Application.Features.Command.Create.Assesment;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void  AddRepository(this IServiceCollection services)
        {

        }

        public static void AddMediatRs(this IServiceCollection services)
        {
            services.AddMediatR(mR => mR.RegisterServicesFromAssemblies(typeof(TextAssessmentHandler).Assembly));
        }

    }
}
