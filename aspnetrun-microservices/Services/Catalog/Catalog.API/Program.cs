using Catalog.API.Data;
using Catalog.API.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();

var app = builder.Build();
Configure();

app.Run();

void ConfigureServices()
{
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.API", Version = "v1" });
    });

    builder.Services.AddScoped<ICatelogContext, CatalogContext>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();

    var mongoConnectionString = builder.Configuration["DatabaseSettings:ConnectionString"];
    builder.Services.AddSingleton(sp => new MongoClient(mongoConnectionString));

    builder.Services.AddHealthChecks()
    .AddMongoDb(sp => sp.GetRequiredService<MongoClient>(),
                name: "MongoDb Health",
                failureStatus: HealthStatus.Degraded,
                tags: ["database", "mongodb"]);
}

void Configure()
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
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