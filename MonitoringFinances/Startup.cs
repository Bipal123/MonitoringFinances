using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitoringFinances.Data;
using MonitoringFinances.Models;
using MonitoringFinances.Models.Identity;
using System;
using System.Threading.Tasks;

namespace MonitoringFinances
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
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders().AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTY0MTE5QDMxMzkyZTM0MmUzMEE1OTM4R01nU0dQYmNRV01tWVNsTFRtUDM2VUNOcnh0ODM2dWtXTnVvcTg9");

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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRolesAndTestAccounts(serviceProvider).Wait();
        }

        private static async Task CreateRolesAndTestAccounts(IServiceProvider serviceProvider)
        {

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { WebConstant.AdminRole, WebConstant.StandardUserRole };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _adminTest = await UserManager.FindByEmailAsync(WebConstant.TestAdminEmail);
            if (_adminTest == null)
            {
                var adminTest = new ApplicationUser
                {
                    UserName = WebConstant.TestAdminEmail,
                    Email = WebConstant.TestAdminEmail,
                    FirstName = "Admin",
                    LastName = "Account"
                };

                var createAdminResult = await UserManager.CreateAsync(adminTest, WebConstant.TestAdminPassword);
                if (createAdminResult.Succeeded)
                    await UserManager.AddToRoleAsync(adminTest, WebConstant.AdminRole);
            }

            var _standardUserTest = await UserManager.FindByEmailAsync(WebConstant.TestStandardUserEmail);
            if (_standardUserTest == null)
            {
                var standardUserTest = new ApplicationUser
                {
                    UserName = WebConstant.TestStandardUserEmail,
                    Email = WebConstant.TestStandardUserEmail,
                    FirstName = "StandardUser",
                    LastName = "Account"
                };

                var createStandardUserResult = await UserManager.CreateAsync(standardUserTest, WebConstant.TestStandardUserPassword);
                if (createStandardUserResult.Succeeded)
                    await UserManager.AddToRoleAsync(standardUserTest, WebConstant.StandardUserRole);
            }

        }
    }
}
