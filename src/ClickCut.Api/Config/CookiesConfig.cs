using Microsoft.AspNetCore.Authentication.BearerToken;

namespace ClickCut.Api.Config;

public static class CookiesConfig
{
	public static string AccessToken = "_access_token";
	public static string RefreshToken = "_refresh_token";
	public static string BusinessToken = "_business_token";

}