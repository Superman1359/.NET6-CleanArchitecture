﻿using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Repository;
using CleanArchitecture.Repository.Base;
using CleanArchitecture.Repository.DatabaseContext;
using CleanArchitecture.Service;
using DotNetCore.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace CleanArchitecture.Api.Helpers;

public static class Extension
{
    public static void RegisterSeriLog()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("serilogconfig.json")
            .Build();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();
    }

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    public static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1",
                new OpenApiInfo
                { Title = "CleanArchitecture.Net6", Version = "v1", Description = "Onion Architecture Using .Net6" });
        });
    }

    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(option =>
            option.AddPolicy("AppPolicy", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
    }

    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(services.GetConnectionString("DefaultConnection"));
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IWeatherForecastService, WeatherForecastService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
    }

    public static void RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(x => x.FullName!.StartsWith(nameof(CleanArchitecture))));    // ToDo: Change "CleanArchitecture" to "YOUR_PROJECT_BASE_NAMESPACE"
    }
}