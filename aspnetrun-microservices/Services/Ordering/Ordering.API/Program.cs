using EventBus.Messages.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using System;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();

var app = builder.Build();
Configure();

app.Run();

void ConfigureServices()
{
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddMassTransit(config =>
    {
        config.AddConsumer<BasketCheckoutConsumer>();
        config.UsingRabbitMq((context, configuration) =>
        {
            configuration.Host(builder.Configuration["EventBusSettings:HostAddress"]);
            configuration.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
            {
                c.ConfigureConsumer<BasketCheckoutConsumer>(context);
            });
        });
    });

    builder.Services.Configure<MassTransitHostOptions>(
       options =>
       {
           options.WaitUntilStarted = true;
           options.StartTimeout = TimeSpan.FromSeconds(30);
           options.StopTimeout = TimeSpan.FromMinutes(1);
       });

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddScoped<BasketCheckoutConsumer>();

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
    });
    builder.Services.AddHealthChecks()
         .AddDbContextCheck<OrderContext>();
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
void Configure()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
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