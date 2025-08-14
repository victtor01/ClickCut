namespace ClickCut.Api.Extensions;

public static class HttpContextExtensions
{
	private static readonly CookieOptions cookieOptions = new()
	{
		HttpOnly = true,
		Secure = false,
		SameSite = SameSiteMode.Strict,
		Expires = DateTime.UtcNow.AddDays(7)
	};

	public static void SetHttpOnly(this HttpContext httpContext, string key, string value)
	{
		var cookiesResponse = httpContext.Response.Cookies;
		cookiesResponse.Append(key, value, cookieOptions);
	}

	public static void ClearAllCookies(this HttpContext httpContext)
	{
		var cookieKeys = httpContext.Request.Cookies.Keys;

		foreach (var key in cookieKeys)
		{
			var expiredCookieOptions = new CookieOptions()
			{
				HttpOnly = true,
				Secure = true,
				Path = "/",
				Expires = DateTime.UtcNow.AddDays(-1),
				SameSite = SameSiteMode.Lax,
			};

			httpContext.Response.Cookies.Append(key, "", expiredCookieOptions);
		}
	}
}