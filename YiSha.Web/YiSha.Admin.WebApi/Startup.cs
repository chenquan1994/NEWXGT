using System.Text;
using System.IO;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Business.AutoJob;
using YiSha.Admin.WebApi.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using YiSha.Admin.WebApi.Tools;
using Microsoft.EntityFrameworkCore;
using YiSha.Model;
using YiSha.Model.Result.SystemManage;

namespace YiSha.Admin.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 跨域配置名称
        /// </summary>
        private const string DefaultCorsPolicyName = "AllowCross";
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            #region JWT认证
            services.AddAuthentication(options => {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                 .AddJwtBearer(options => {
                     //主要是jwt  token参数设置
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         //颁发者
                         ValidateIssuer = true,
                         ValidIssuer = Configuration["JWT:Issuer"],
                         //被授权者
                         ValidateAudience = true,
                         ValidAudience = Configuration["JWT:Audience"],
                         //秘钥
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"])),
                         //是否验证失效时间【使用当前时间与Token的Claims中的NotBefore和Expires对比】
                         ValidateLifetime = true,
                         ClockSkew = TimeSpan.FromMinutes(5)//允许的服务器时间偏移量【5分钟】
                     };


                     options.Events = new JwtBearerEvents
                     {
                         OnChallenge = async context =>
                         {
                             var shuzu = new
                             {
                                 msg = "未授权",
                                 state = "error"
                             };

                             context.HandleResponse();

                             context.Response.ContentType = "application/json;charset=utf-8";
                             if (context.Response.StatusCode == 401)
                             {
                                 context.Response.StatusCode = StatusCodes.Status200OK;

                             }

                             await context.Response.WriteAsync(JsonConvert.SerializeObject(shuzu));

                         }
                     };


                 });



            #endregion
            services.AddDbContext<MiaContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DBConnectionString")));
            // services.AddOptions();
            services.AddCors(options =>
            {
                options.AddPolicy("cors",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                //builder => builder.AllowAnyMethod().AllowAnyHeader()    
                );
            });
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.AllowAnyOrigin()
            //               .AllowAnyMethod()
            //               .AllowAnyHeader();
            //    });
            //});

            services.AddControllers(optipns =>
            {

                optipns.Filters.Add<UsersAtuhorizeAttribute>();
            });

            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());

            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });


            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
            });


            services.AddMemoryCache();

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //注册Encoding

            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();

            GlobalContext.Services = services;

            GlobalContext.Configuration = Configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
            //    RequestPath = "/Files"
            //});


            string resource = Path.Combine(env.ContentRootPath, "Resource");


            FileHelper.CreateDirectory(resource);


            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl

            });

            app.UseMiddleware(typeof(GlobalExceptionMiddleware));


            //同时开启http，https/ app.UseHsts();

            #region JWT
            app.UseRouting();
            //配置跨域
            //app.UseCors();

            app.UseCors("cors");

            //1.先开启认证
            app.UseAuthentication();
            //2.再开启授权
            app.UseAuthorization();

            #endregion

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
            });

            new JobCenter().Start();

        }
    }
}