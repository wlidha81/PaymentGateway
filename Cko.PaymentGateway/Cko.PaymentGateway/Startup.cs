using AutoMapper;
using Cko.PaymentGateway.BankSimulator;
using Cko.PaymentGateway.Business;
using Cko.PaymentGateway.Business.Mappers;
using Cko.PaymentGateway.Business.Services.Implementations;
using Cko.PaymentGateway.Data.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cko.PaymentGateway
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddLogging();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddResponseCaching();

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddValidatorsFromAssemblyContaining<PaymentRequestValidator>(ServiceLifetime.Singleton);
            services.AddSingleton<IAcquiringBank, AcquiringBank>();
            //services.AddHttpClient<IAcquiringBank, AcquiringBank>();
            services.AddHttpClient();
            services.AddScoped<IPaymentService, PaymentService>();
            
            services.AddHealthChecks();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cko PaymentGateway V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseApiVersioning();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health").RequireHost("wwww.checkout.com");//checkout.com took as an example of 3rdparty organization;
            });
        }
    }
}
