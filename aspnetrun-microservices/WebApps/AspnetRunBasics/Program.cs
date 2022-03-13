using AspnetRunBasics.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();

var app = builder.Build();
Configure();

app.Run();

// This method gets called by the runtime. Use this method to add services to the container.
void ConfigureServices()
{
    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
    builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
    builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
        c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
    builder.Services.AddHealthChecks()
       .AddUrlGroup(new Uri($"{builder.Configuration["ApiSettings:GatewayAddress"]}/hc"), "Ocelot API Gw", HealthStatus.Degraded);
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
void Configure()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        //app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    });
}