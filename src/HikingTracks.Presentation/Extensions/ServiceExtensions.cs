﻿using HikingTracks.Application;
using HikingTracks.Application.Factories;
using HikingTracks.Application.Interfaces;
using HikingTracks.Application.Service;
using HikingTracks.Domain;
using HikingTracks.Domain.Interfaces;
using HikingTracks.Infrastructure;
using HikingTracks.Infrastructure.Repositories;
using HikingTracks.LoggerService;
using Microsoft.EntityFrameworkCore;

namespace HikingTracks.Presentation.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                    builder
                    .AllowAnyOrigin()
                    .WithMethods("POST", "GET", "PUT", "DELETE"));
        });
    }

    public static void ConfigureRepositoryFactory(this IServiceCollection services) =>
        services.AddScoped<IRepositoryFactory, RepositoryFactory>();   

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.ConfigureRepositoryFactory();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    } 

    public static void ConfigureServiceFactory(this IServiceCollection services) =>
        services.AddScoped<IServiceFactory, ServiceFactory>();   

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.ConfigureServiceFactory();
        services.AddScoped<IServiceManager, ServiceManager>();
    }
    
    public static void ConfigureDbContext(this IServiceCollection services)
    {
        services.AddDbContext<HikingTracksContext>(options =>
        {
            options.UseNpgsql("Host=localhost;Username=postgres;Password=root;Database=HikingTracks");
        });
    }

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>(); 
}
