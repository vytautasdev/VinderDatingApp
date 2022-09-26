using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration config;

        public Startup(IConfiguration config)
        {
            this.config = config;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppServices(config);
            services.AddControllers();
            services.AddCors();
            services.AddIdentityServices(config);
            services.AddSignalR();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "WebAPIv5", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(cors =>
                cors.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin()
                    // .WithOrigins("https://localhost:4200"));
                    .WithOrigins("https://vinderapp.herokuapp.com"));

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PresenceHub>("hubs/presence");
                endpoints.MapHub<MessageHub>("hubs/message");
                endpoints.MapFallbackToController("Index", "Fallback");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
        }
    }
}