var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var application = builder.Build();

if (!application.Environment.IsDevelopment())
{
    application.UseExceptionHandler("/Error");
    application.UseHsts();
}

application.UseHttpsRedirection();
application.UseStaticFiles();
application.UseRouting();
application.UseAuthorization();
application.MapRazorPages();

application.Run();
