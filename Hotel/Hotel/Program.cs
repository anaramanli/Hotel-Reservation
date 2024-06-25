using Hotel.DAL;
using Hotel.Interfaces;
using Hotel.Models;
using Hotel.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Hotel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<HotelDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt => {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.SignIn.RequireConfirmedEmail = true;

            }).AddEntityFrameworkStores<HotelDBContext>().AddDefaultTokenProviders(); 
            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection(nameof(Stripe)));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:Secretkey"];

            app.MapControllerRoute(name: "roomDetail", pattern: "room/details/{id?}", defaults: new { controller = "Room", action = "Details" });
            app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Slider}/{action=Index}/{id?}");
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
