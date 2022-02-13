using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Data.Models_QLTour;
using Data.Repository;
using GleamTech.AspNet.Core;
using IntranetFolder.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace IntranetFolder
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
            services.AddDbContext<qltaikhoanContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))/*.EnableSensitiveDataLogging()*/);
            services.AddDbContext<qltourContext>(options => options.UseSqlServer(Configuration.GetConnectionString("QLTourConnection"))/*.EnableSensitiveDataLogging()*/);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // AddAutoMapper
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFolderUserReprository, FolderUserReprository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<ITinhRepository, TinhRepository>();
            services.AddTransient<IThanhPho1Repository, ThanhPho1Repository>();
            services.AddTransient<IDmdiemtqRepository, DmdiemtqRepository>();
            services.AddTransient<IDanhGiaNhaCungUngRepository, DanhGiaNhaCungUngRepository>();
            services.AddTransient<ILoaiDvRepository, LoaiDvRepository>();
            services.AddTransient<IDanhGiaNhaHangRepository, DanhGiaNhaHangRepository>();
            services.AddTransient<IDanhGiaKhachSanRepository, DanhGiaKhachSanRepository>();
            services.AddTransient<IDanhGiaLandTourRepository, DanhGiaLandTourRepository>();
            services.AddTransient<IErrorRepository, ErrorRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // services
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<ITinhTPService, TinhTPService>();
            services.AddTransient<IThanhPho1Service, ThanhPho1Service>();
            services.AddTransient<IDiemTQService, DiemTQService>();
            services.AddTransient<IDanhGiaNhaCungUngService, DanhGiaNhaCungUngService>();
            services.AddTransient<IDanhGiaNhaHangService, DanhGiaNhaHangService>();
            services.AddTransient<IDanhGiaKhachSanService, DanhGiaKhachSanService>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
            //Add GleamTech to the ASP.NET Core services container.
            //----------------------
            services.AddGleamTech();
            //----------------------
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Register GleamTech to the ASP.NET Core HTTP request pipeline.
            //----------------------
            app.UseGleamTech();
            //----------------------
            ////Set this property only if you have a valid license key, otherwise do not
            ////set it so FileUltimate runs in trial mode.
            //FileUltimateConfiguration.Current.LicenseKey = "QQJDJLJP34...";

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            var supportedCultures = new[] { new CultureInfo("en-AU") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-AU"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}