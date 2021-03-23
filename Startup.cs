using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicCoreApplication.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BasicCoreApplication
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
			services.AddControllers();

			services.AddSwaggerGen(options => {
				//it is use for the middleware of swagger to pass the token for authorization in the api testing
				//options.SwaggerDoc("v1", new Info { Title = "Some API", Version = "v1" });
				options.AddSecurityDefinition("bearer", new ApiKeyScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					In = "header",
					Name = "Authorization",
					Type = "apiKey"
				});
				var securityRequirement = new OpenApiSecurityRequirement
				{
					{
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "bearer"
								}
							},
							new string[] {}
					}
				};
				options.AddSecurityRequirement(securityRequirement);
			});

			//it requires to install the using Microsoft.EntityFrameworkCore.SqlServer package
			services.AddDbContext<DatabaseContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = Configuration["Jwt:Issuer"],
					ValidAudience = Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseContext dbContext)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseHttpsRedirection(); //if use swagger then  no need to use the default http redirection

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = ""; //run the swagger on default route
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test APi with Entity Framework"); //open the swagger.json which have all the instance of api
			});

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}

	internal class ApiKeyScheme : OpenApiSecurityScheme
	{
		public string Description { get; set; }
		public string In { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
	}
}
