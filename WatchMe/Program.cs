using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WatchMe.Data;
using Microsoft.AspNetCore.Identity;
using WatchMe.Identity.Data;
using WatchMe.Models;

namespace WatchMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddDbContext<WatchMeContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("WatchMeContext") ?? throw new InvalidOperationException("Connection string 'WatchMeContext' not found.")));

            builder.Services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<WatchMeContext>();

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();;

            app.UseAuthorization();


            app.MapControllers();

            WatchMeContext? context = app.Services.CreateScope().ServiceProvider.GetService<WatchMeContext>();
            RoleManager<AppRole>? roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<AppRole>>();
            UserManager<AppUser>? userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<AppUser>>();
            DBInitializer dBInitializer = new DBInitializer(context, roleManager, userManager);

            app.Run();
        }
    }
}
