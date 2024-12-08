using Chat.API.Data.Interfaces;
using Chat.API.Data.Repositories;
using Chat.API.Options;
using Microsoft.EntityFrameworkCore;

namespace Chat.API.Data;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddDataContext(this IServiceCollection services,
        DataContextOptions dataContextOptions)
    {
        services.AddPooledDbContextFactory<ChatDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(dataContextOptions.ConnectionString, opts =>
                opts.EnableRetryOnFailure(
                    dataContextOptions.MaxRetryCountOnFailure,
                    TimeSpan.FromSeconds(dataContextOptions.RetryDelayInSeconds),
                    null
                )
            )
        );

        services.AddDbContextPool<DbContext, ChatDbContext>(optionsBuilder =>
            optionsBuilder.UseNpgsql(dataContextOptions.ConnectionString,
                opts =>
                    opts.EnableRetryOnFailure(
                        dataContextOptions.MaxRetryCountOnFailure,
                        TimeSpan.FromSeconds(dataContextOptions.RetryDelayInSeconds),
                        null
                    )
            )
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<UserRepository>();
        services.AddScoped<ChatRepository>();
        services.AddScoped<MessageRepository>();
        
        return services;
    }
}