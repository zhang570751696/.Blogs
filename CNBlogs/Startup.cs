using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using CNBlogs.Common;
using CNBlogs.Impl;
using CNBlogs.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace CNBlogs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<ICachingProvider, MemoryCachingProvider>();
            services.AddScoped<IDateTimeService, DateTimeService>();

            // config swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "asp.net core web api",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Name = "Rick",
                        Email = "570751696@qq.com",
                        Url = ""
                    }
                });

                // get the object of the config info. config swagger ui 
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "CNBlogs.xml");
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<AddAuthTokenHeaderParameter>();
            });

            return this.GetAutofacServiceProvider(services);

        }

        private IServiceProvider GetAutofacServiceProvider(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<QCachingInterceptor>();
            var filesNames = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.AllDirectories);
            var assemblyNames = filesNames.Select(AssemblyLoadContext.GetAssemblyName);
            List<Assembly> assemblies = new List<Assembly>();
            foreach (AssemblyName assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                // scenario 1
                builder.RegisterAssemblyTypes(assembly)
                             .Where(type => typeof(IQCaching).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                             .AsImplementedInterfaces()
                             .InstancePerLifetimeScope()
                             .EnableInterfaceInterceptors()
                             .InterceptedBy(typeof(QCachingInterceptor));
            }

            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // user swagger
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                // do limit info description
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "web api V1");
                x.ShowExtensions();
                x.ValidatorUrl(null);
            });

            app.UseMvc();
        }
    }
}
