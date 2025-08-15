using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClickCut.Api.Config;
using ClickCut.Api.Middlewares;
using ClickCut.Application.Ports.In;
using ClickCut.Application.Ports.Out;
using ClickCut.Application.Services;
using ClickCut.Infra.Adapters.In.Services;
using ClickCut.Infra.Adapters.Out.Persistence.Repositories;
using ClickCut.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

namespace ClickCut.Api.Extensions;

public static class BuilderExtension
{

	private static JsonSerializerOptions _jsonOption = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
	public static void AddServicesExtension(this IServiceCollection services)
	{
		// -- services -- 
		services.AddScoped<IUsersServicePort, UsersServiceImplements>();
		services.AddScoped<IValidationServicePort, ValidationServiceImplements>();
		services.AddScoped<IAuthServicePort, AuthServiceImplements>();
		services.AddScoped<IPasswordServicePort, PasswordServiceImplements>();
		services.AddScoped<IBusinessServicePort, BusinessServiceImplements>();
		services.AddSingleton<IJwtServicePort, JwtService>();
		// -- repositories --
		services.AddScoped<IUsersRepositoryPort, UsersRepositoryImplements>();
		services.AddScoped<IBusinessRepositoryPort, BusinessRepositoryImplements>();
	}

	public static void AddAuthConfig(this WebApplicationBuilder builder)
	{
		builder.Services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ClockSkew = TimeSpan.Zero,
				ValidIssuer = builder.Configuration["Jwt:Issuer"],
				ValidAudience = builder.Configuration["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
			};

			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					context.Token = context.Request.Cookies[CookiesConfig.AccessToken];
					return Task.CompletedTask;
				},
				OnChallenge = context =>
				{
					context.HandleResponse();
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					context.Response.ContentType = "application/json";

					UnauthorizedException unauthorizedException = new UnauthorizedException("Sua sessão de acesso é inválido, expirou ou não foi fornecido.");

					string result = JsonSerializer.Serialize(
						new
						{
							type = unauthorizedException.Type,
							message = unauthorizedException.Message,
							errors = unauthorizedException.Errors,
						},
						_jsonOption
					);

					return context.Response.WriteAsync(result);
				}
			};
		});
	}

	public static void AddSerilogConfiguration(this WebApplicationBuilder builder)
	{
		builder.Logging.ClearProviders();

		var logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
			.Enrich.FromLogContext()
			.ReadFrom.Configuration(builder.Configuration)
			.WriteTo.Console(outputTemplate:
				"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
			.CreateLogger();

		builder.Host.UseSerilog(logger);
	}

	public static IApplicationBuilder UseSessionMiddleware(this WebApplication builder)
	{
		return builder.UseMiddleware<SessionMiddleware>();
	}
}