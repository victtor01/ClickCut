using Microsoft.AspNetCore.Mvc;

namespace ClickCut.Api.Extensions;

public static class MvcExtensions
{
	public static IMvcBuilder AddCustomInvalidModelStateResponseFactory(this IMvcBuilder builder)
	{
		builder.ConfigureApiBehaviorOptions(options =>
		{
			options.InvalidModelStateResponseFactory = context =>
			{
				var errors = context.ModelState
					.Where(e => e.Value?.Errors.Count > 0)
					.ToDictionary(
						kvp => kvp.Key,
						kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
					);

				var errorResponse = new
				{
					type = "ValidationError",
					message = "Um ou mais erros de validação ocorreram.",
					errors
				};

				var badRequestResult = new BadRequestObjectResult(errorResponse);

				badRequestResult.ContentTypes.Add("application/json");

				return badRequestResult;
			};
		});

		return builder;
	}
}