using AspnetRunBasicBlazor.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();

var app = builder.Build();
Configure();

app.Run();

void ConfigureServices()
{
    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
                   c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
    builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
    builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
    builder.Services.AddHealthChecks()
                   .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:GatewayAddress"]}/hc"), "Ocelot API Gw", HealthStatus.Degraded);
}

void Configure()
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
    }

    app.UseStaticFiles();

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapBlazorHub();
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    });
    app.MapFallbackToPage("/_Host");
}