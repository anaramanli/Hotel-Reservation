using Hotel.DAL;
using Microsoft.EntityFrameworkCore;

namespace Hotel
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddDbContext<HotelDBContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
			}
			app.UseStaticFiles();
			app.MapControllerRoute(name: "areas",pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}");
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}
