using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGatewayApi.Api;
using PaymentGatewayApi.Logging;
using PaymentGatewayApi.Mapper;
using PaymentGatewayApi.Service;
using Polly;

namespace PaymentGatewayApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddLogging();
            services.AddSingleton<IBankPaymentMapper, BankPaymentMapper>();
            services.AddSingleton<IApplicationLogger, ApplicationLogger>();
            services.AddHttpClient<IBankApiClient, BankApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44389");
            })
                .AddTransientHttpErrorPolicy(policyBuilder =>
                    policyBuilder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i))))
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(5, TimeSpan.FromSeconds(20)));;
            services.AddTransient<IPaymentGatewayService, PaymentGatewayService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
