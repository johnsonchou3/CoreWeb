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
    /// Core Startup 類別
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup 的設置
        /// </summary>
        /// <param name="configuration">傳入本身的configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 定義程式的config
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 跑程式時會使用, 可新增服務
        /// </summary>
        /// <param name="services">存取config</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ConcurrentDictionary<string,CalData>>();
            services.AddControllers();
        }

        /// <summary>
        /// 跑程式時會使用, 可修改HTTP request 
        /// </summary>
        /// <param name="app">程式的IApplicationBuilder</param>
        /// <param name="env">程式的IWebHostEnvironment</param>
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
