using AutoMapper;
using MessageBoard.Data;
using MessageBoard.Helpers;
using MessageBoard.Repositories;
using MessageBoard.Repositories.Interface;
using MessageBoard.Services;
using MessageBoard.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBoard
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
            services.AddDbContext<MessageBoardContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            #region AutoMapper

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            #endregion

            #region IOC

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGuestbookRepository, GuestbookRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();

            services.AddScoped<IGuestbookService, GuestbookService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMailService, MailService>();

            services.AddSingleton<IConfigHelper>(new ConfigHelper(Configuration.GetSection("WebConfig")));

            Misc.WebConfig webConfig = Configuration.GetSection("WebConfig").Get<Misc.WebConfig>();
            services.AddSingleton(webConfig);

            #endregion

            #region Session

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            #endregion

            #region MailKit

            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    //get options from secrets.json
                    Server = webConfig.MailServer.Server,
                    Port = Convert.ToInt32(webConfig.MailServer.Port),
                    SenderName = webConfig.MailServer.SenderName,
                    SenderEmail = webConfig.MailServer.SenderEmail,

                    // can be optional with no authentication 
                    Account = webConfig.MailServer.Account,
                    Password = webConfig.MailServer.Password,
                    // enable ssl or tls
                    Security = true
                });
            });

            #endregion

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
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
            app.UseStaticFiles();

            app.UseStaticHttpContextAccessor();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
