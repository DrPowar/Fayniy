using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoursesStore.Data;
using CoursesStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.WebSockets;
using CoursesStore.Services;

namespace CoursesStore
{
    public class Program
    {
        public static async Task Main(string[] args)
        { 
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CoursesStoreContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("CoursesStoreContext"),
                                 ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("CoursesStoreContext"))));

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<CoursesStoreContext>()
            //    .AddDefaultTokenProviders();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CoursesStoreContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddTransient<IBraintreeService, BraintreeService>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login"; // Change this to your login path
                options.AccessDeniedPath = "/Account/AccessDenied"; // Change this to your access denied path
                options.SlidingExpiration = true;
            });


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                SeedData.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Courses}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.UseRequestLocalization("uk-UA");

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string name = "admin";

                string password = "Admin1234.";

                if(await userManager.FindByNameAsync(name) == null)
                {
                    var user = new ApplicationUser();
                    user.UserName = name;
                    user.Password = password;
                    user.EmailConfirmed = true;

                    await userManager.CreateAsync(user, password);

                    userManager.AddToRoleAsync(user, "Admin");
                }
            }

            app.Run();
        }
    }
}