using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace CoreWeb
{
    /// <summary>
    /// Core Startup ���O
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup ���]�m
        /// </summary>
        /// <param name="configuration">�ǤJ������configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// �w�q�{����config
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// �]�{���ɷ|�ϥ�, �i�s�W�A��
        /// </summary>
        /// <param name="services">�s��config</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ConcurrentDictionary<string,CalData>>();
            services.AddControllers();
        }

        /// <summary>
        /// �]�{���ɷ|�ϥ�, �i�ק�HTTP request 
        /// </summary>
        /// <param name="app">�{����IApplicationBuilder</param>
        /// <param name="env">�{����IWebHostEnvironment</param>
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
        }
    }
}
