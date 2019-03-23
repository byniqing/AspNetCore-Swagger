using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwaggerApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static SwaggerApi.SwaggerHelper.CustomApiVersion;

namespace SwaggerApi
{
    public class Startup
    {
        /// <summary>
        /// Api版本提者信息，
        /// 依赖包：Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
        /// </summary>
        private IApiVersionDescriptionProvider provider;

        private const string ApiName = "Manager.Core";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //in-database 用于测试
            services.AddDbContext<Context>(opt => opt.UseInMemoryDatabase("StudentList"));

            services.AddSwaggerGen(opt =>
            {
                //遍历出全部的版本，做文档信息展示
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    opt.SwaggerDoc(version, new Info
                    {
                        // {ApiName} 定义成全局变量，方便修改
                        Version = version,
                        Title = $"{ApiName} 接口文档",
                        Description = $"{ApiName} HTTP API " + version,
                        TermsOfService = "None",
                        Contact = new Contact
                        {
                            Name = "糯米粥",
                            Email = "nsky@cnblogs.com",
                            Url = "http://www.cnblogs.com/nsky"
                        }
                    });
                    // 按相对路径排序，
                    opt.OrderActionsBy(o => o.RelativePath);

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    opt.IncludeXmlComments(xmlPath, true);
                });

                opt.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = "http://petstore.swagger.io/oauth/dialog",
                    Scopes = new Dictionary<string, string>
                    {
                        { "readAccess", "Access read operations" },
                        { "writeAccess", "Access write operations" }
                    }
                });
            });

            #region 如果不用版本控制
            //services.AddSwaggerGen(s =>
            //   {
            //       s.SwaggerDoc("v1", new Info
            //       {
            //           Title = "SwaggerApi测试",
            //           Version = "v1",
            //           Description = "接口文档描述",
            //           Contact = new Contact
            //           {
            //               Name = "糯米粥",
            //               Email = "nsky@cnblogs.com",
            //               Url = "http://www.cnblogs.com/nsky"
            //           }
            //       });

            //       var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //       var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //       s.IncludeXmlComments(xmlPath, true);
            //   });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //允许中间件作为JSON端点为生成的Swagger提供服务。
            app.UseSwagger();

            //配置swaggerui信息
            app.UseSwaggerUI(options =>
            {
                #region 单个版本控制
                //options.SwaggerEndpoint("/swagger/v1/swagger.json", "Api_v1");
                //s.RoutePrefix = string.Empty; 
                #endregion

                #region 多版本控制

                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                    options.RoutePrefix = string.Empty;
                });

                //foreach (var description in provider.ApiVersionDescriptions)
                //{
                //    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                //}
                #endregion
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
