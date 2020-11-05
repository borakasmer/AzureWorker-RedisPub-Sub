using System;
using Entity.Models.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Exchanges;

namespace ExchangeService
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
            services.AddDbContext<BlackJackContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));        
            services.AddTransient<IExchangeServices, ExchangeServices>();
            services.AddTransient<IRedisCacheService, RedisCacheService>();
            
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations(); //Amaç Swagger'da Açıklama Girmek
                /*
                    CoreSwagger adı ile Başlık, versiyon, açıklama özellikleri ile service’e eklenir.
                */

                c.SwaggerDoc("CoreSwagger", new OpenApiInfo
                {
                    Title = "Swagger on ASP.NET Core",
                    Version = "1.0.0",
                    Description = "VBT Web Api",
                    TermsOfService = new Uri("http://swagger.io/terms/")
                });
            });

            //Automapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /* 
                Burada swagger için kullanılacak “json” dosyasının kaydedileceği yer tanımlanmaktadır.
                NOT: “/CoreSwagger” olarak yazılan kısım ==> yukarıda ConfigureServices’de tanımlanan “c.SwaggerDoc(“CoreSwagger“, new OpenApiInfo” ile 
                aynı olmak zorundadır.Swagger’ın isim parametresi olarak kullanılmaktadır.
            */
            app.UseSwagger()
           .UseSwaggerUI(c =>
           {
               //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
               c.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "Swagger Test .Net Core");

               //TODO: Or alternatively use the original Swagger contract that's included in the static files
               // c.SwaggerEndpoint("/swagger-original.json", "Swagger Petstore Original");
           });
        }
    }
}
