using Livestreaming.Application.Services;
using Livestreaming.Infrastructure.ORM;
using Livestreaming.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Livestreaming.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LivestreamContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LivestreamDB"),
                                     providerOptions =>
                                     {
                                         providerOptions.EnableRetryOnFailure(maxRetryCount: 10,
                                                                              maxRetryDelay: TimeSpan.FromSeconds(30),
                                                                              errorNumbersToAdd: null);
                                         providerOptions.MigrationsAssembly("Livestreaming.Infrastructure");
                                     });
            });

            //services.AddControllersWithViews();
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddTransient<ILivestreamRepository, LivestreamRepository>()
                    .AddScoped<LivestreamService>();

            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Livestreaming.API", Version = "v1" });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Livestreaming.API v1"));
            }
            else
            { // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                /*endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");*/

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}