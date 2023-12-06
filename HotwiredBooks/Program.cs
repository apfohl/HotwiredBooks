using HotwiredBooks.Components;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options => options.OutputFormatters.Add(new TurboStreamOutputFormatter()));
builder.Services.AddSingleton<IBooksRepository, MemoryBasedBooksRepository>();

var application = builder.Build();

if (!application.Environment.IsDevelopment())
{
    application.UseExceptionHandler("/Books/Error");
    application.UseHsts();
}

application.UseHttpsRedirection();
application.UseStaticFiles();
application.UseRouting();
application.UseAuthorization();
application.UseMiddleware<HttpMethodOverrideMiddleware>();
application.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}"
);

application.Run();
