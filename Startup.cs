using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LineWebHook {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.Configure<KestrelServerOptions> (options => {
                options.AllowSynchronousIO = true;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(opts => {
                opts.IdleTimeout        = TimeSpan.FromSeconds(20);
                opts.Cookie.HttpOnly    = true;
                opts.Cookie.IsEssential = true;
            });

            services.AddControllers ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseSession();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}