using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAppMySQL.Models;

namespace WebAppMySQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //instead of "localhost" you can use your IP address.
            //For getting you IP Address go to `cmd` ,then type 'ipconfig'
            var host = Configuration["DBHOST"] ?? "localhost";

            var port = Configuration["DBPORT"] ?? "3306";

            var password = Configuration["DBPASSWORD"] ?? "root";


            //StudentDetailContext is Our DB Context Class
            services.AddDbContextPool<StudentDetailContext>(
                    options =>
                    {
                        var connectionString = $"server={host};userid=root;pwd={password};port={port};database=StudentDB";
                        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 11)));
                    });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StudentDetailContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            context.Database.Migrate();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=StudentDetails}/{action=Index}/{id?}");
            });
        }
    }
}
