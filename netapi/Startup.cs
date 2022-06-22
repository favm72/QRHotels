using Common;
using DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using netapi.Hubs;

namespace netapi
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
			services.AddCors();
			services.AddControllers();
			services.AddSignalR();

			services.Configure<MyConfig>(Configuration.GetSection("MyConfig"));			
			services.AddDbContext<MyContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("conn"));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseCors(builder => {
			//	builder.WithOrigins("http://localhost:4200")
			//	.AllowAnyMethod()
			//	.AllowAnyHeader()
			//	.AllowCredentials();
			//});

			app.UseCors(builder => builder
				.AllowAnyHeader()
				.AllowAnyMethod()
				.SetIsOriginAllowed(_ => true)
				.AllowCredentials()
			);

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<OrderHub>("/orderhub");
			});
		}
	}
}
