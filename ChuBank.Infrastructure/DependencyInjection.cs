using ChuBank.Domain.Interfaces;
using ChuBank.Infrastructure.Context;
using ChuBank.Infrastructure.Repositories;
using ChuBank.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuBank.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ChuBankDbContext>(options =>
            options.UseSqlServer(connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IAccountRepository, AccountRepository>();

        services.AddHttpClient<IHolidayService, BrasilApiHolidayService>();

        services.AddMemoryCache();

        return services;
    }
}