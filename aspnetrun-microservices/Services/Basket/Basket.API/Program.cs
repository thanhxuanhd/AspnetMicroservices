using Basket.API.GrpcService;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();

var app = builder.Build();
Configure();

app.Run();

void ConfigureServices()
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["CacheSettings:ConnectionString"];
    });

    builder.Services.AddScoped<IBasketRepository, BasketRepository>();

    builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o =>
    {
        o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
    });
    builder.Services.AddScoped<DiscountGrpcService>();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddMassTransit(config =>
    {
        config.UsingRabbitMq((context, configuration) =>
        {
            configuration.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        });
    });

    builder.Services.Configure<MassTransitHostOptions>(
        options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });
    //builder.Services.AddMassTransitHostedService();

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.API", Version = "v1" });
    });

    builder.Services.AddHealthChecks()
            .AddRedis(builder.Configuration["CacheSettings:ConnectionString"], "Redis Health", HealthStatus.Degraded);
}

void Configure()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
    }

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/hc", new HealthCheckOptions()
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
}