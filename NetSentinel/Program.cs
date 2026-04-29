using Microsoft.EntityFrameworkCore;
using NetSentinel.Data;
namespace NetSentinel

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DbContext kaydı
            builder.Services.AddDbContext<NetSentinelDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Arka planda çalışacak Ping motoru ve HTTP istekleri için gerekli servisler
            builder.Services.AddHttpClient();
            builder.Services.AddHostedService<NetSentinel.Services.PingWorkerService>(); // Eğer klasör adın Services ise
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
