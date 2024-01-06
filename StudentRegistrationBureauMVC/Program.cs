using ApplicationService.Implementations;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace StudentRegistrationBureauMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<StudentRegistrationBureauContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));

            builder.Services.AddDefaultIdentity<AppUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;

                //Other options go here
            })
            .AddEntityFrameworkStores<StudentRegistrationBureauContext>();

            builder.Services.AddRazorPages();

            builder.Services.AddSingleton(typeof(MajorManagementService));
            builder.Services.AddSingleton(typeof(FacultyManagementService));
            builder.Services.AddSingleton(typeof(StudentManagementService));
            builder.Services.AddSingleton(typeof(CourseManagementService));

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
