using ClickCut.Api.Extensions;
using ClickCut.Api.Middlewares;
using ClickCut.Infra.Adapters.Out.Persistence.DatabaseContext;
using ClickCut.Infra.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.AddSerilogConfiguration();
builder.AddAuthConfig();

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.CreateBootstrapLogger();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDatabaseContext>(opt =>
{
	opt.UseNpgsql(connectionString);
});

builder.Services.AddOpenApi();
builder.Services.AddControllers().AddCustomInvalidModelStateResponseFactory();
builder.Services.AddServicesExtension();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.Run();