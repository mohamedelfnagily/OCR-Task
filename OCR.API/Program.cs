using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OCR.BLL.AutoMapper;
using OCR.BLL.Helpers.JWTDataExtractor;
using OCR.BLL.Helpers.LicenseDataExtraction;
using OCR.BLL.Managers.ManagingAuthentication;
using OCR.BLL.Managers.ManagingCarsData;
using OCR.BLL.Managers.ManagingUsers;
using OCR.DAL.Data.Context;
using OCR.DAL.Data.Models;
using OCR.DAL.Repository.Non_Generic.CarsDataRepo;
using OCR.DAL.Repository.Non_Generic.UsersRepo;
using System.IdentityModel.Tokens.Jwt;

namespace OCR.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Cors Configuration
			string allowPolicy = "AllowAll";
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(allowPolicy, p =>
				{
					p.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true);
				});
			});
			#endregion

			#region Context Configuration
			builder.Services.AddDbContext<ApplicationDbContext>(opt =>
			{
				opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			#endregion

			#region Identity Configuration
			builder.Services.AddIdentity<User, IdentityRole>(opt =>
			{
				opt.User.RequireUniqueEmail = true;
				opt.Password.RequireNonAlphanumeric = false;
				opt.Lockout.AllowedForNewUsers = false;
				opt.Lockout.MaxFailedAccessAttempts = 3;
				opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
			}).AddEntityFrameworkStores<ApplicationDbContext>();
			#endregion

			#region Repos Configuration
			builder.Services.AddScoped<IUserRepository,UserRepository>();
			builder.Services.AddScoped<ICarDataRepository,CarDataRepository>();

			#endregion

			#region Managers Configuration
			builder.Services.AddScoped<IAuthenticationManager,AuthenticationManager>();
			builder.Services.AddScoped<ICarDataManager,CarDataManager>();
			builder.Services.AddScoped<IManageUser,ManageUser>();
			builder.Services.AddScoped<ILicenseData, LicenseData>();
			#endregion

			#region Automapper configuration
			builder.Services.AddAutoMapper(typeof(AutoMapperProfiler).Assembly);
			#endregion

			#region Authentication Configs
			builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "Default";
				options.DefaultChallengeScheme = "Default";
			}).AddJwtBearer("Default", options =>
			{
				SymmetricSecurityKey key = JWTData.getKey(builder.Configuration);
				options.TokenValidationParameters = new TokenValidationParameters
				{
					IssuerSigningKey = key,
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
			#endregion

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}