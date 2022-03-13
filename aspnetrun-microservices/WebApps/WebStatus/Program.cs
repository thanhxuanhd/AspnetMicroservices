using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices();

var app = builder.Build();
Configure();

app.Run();

void ConfigureServices()
{
    builder.Services.AddHealthChecksUI().AddInMemoryStorage();
    builder.Services.AddRazorPages();
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
        app.UseExceptionHandler("/Error");
    }

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecksUI();

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    });
}