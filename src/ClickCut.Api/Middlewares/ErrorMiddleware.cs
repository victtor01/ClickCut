using System.Net;
using System.Text.Json;
using ClickCut.Shared.Exceptions;
using ClickCut.Shared.Extensions;

namespace ClickCut.Api.Middlewares;

public class ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
{
	private readonly RequestDelegate _next = next;
	private readonly ILogger<ErrorMiddleware> _logger = logger;

	public async Task InvokeAsync(HttpContext context) =>
		await _next(context).Catch(ex => HandleExceptionAsync(context, ex));

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var response = context.Response;
		response.ContentType = "application/json";

		if (exception is ErrorInstance errorInstance)
		{
			response.StatusCode = errorInstance.StatusCode;
			string result = JsonSerializer.Serialize(
			  new
			  {
				  type = errorInstance.Type,
				  message = errorInstance.Message,
				  errors = errorInstance.Errors,
			  }
			);

			return response.WriteAsync(result);
		}
		else
		{
			_logger.LogError(exception, "Ocorreu um erro");
			response.StatusCode = (int)HttpStatusCode.InternalServerError;
			var defaultResult = JsonSerializer.Serialize(
			  new { error = "Internal Server Error", message = "Houve um erro desconhecido!" }
			);
			return response.WriteAsync(defaultResult);
		}
	}
}