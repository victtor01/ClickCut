using System.Security.Claims;
using ClickCut.Application.Ports.In;
using ClickCut.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ClickCut.Infra.Settings;
using System.Text;
using ClickCut.Domain.Enums;
using ClickCut.Shared.Utils;

namespace ClickCut.Infra.Adapters.In.Services;

public class JwtService(IOptions<JwtSettings> settings) : IJwtServicePort
{
	private readonly JwtSettings _jwtSettings = settings.Value;

	public string Generate(User user)
		=> Generate(GetClaimsForUser(user), _jwtSettings.ExpirationInHours);

	public string Generate(User user, int minutes)
		=> Generate(GetClaimsForUser(user), minutes);

	public string Generate(Business business)
		=> Generate(GetClaimsForBusiness(business), _jwtSettings.ExpirationInHours);

	public string Generate(Business business, int minutes)
		=> Generate(GetClaimsForBusiness(business), minutes);

	public string Generate(IEnumerable<Claim> claims, int minutes)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = CreateSigningKey();

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Issuer = _jwtSettings.Issuer,
			Audience = _jwtSettings.Audience,
			Expires = DateTime.UtcNow.AddMinutes(minutes),
			SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
		};

		var securityToken = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(securityToken);
	}

	public ClaimsPrincipal? GetPrincipalFromToken(string token, bool validateLifetime)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var validationParameters = GetTokenValidationParameters(validateLifetime: validateLifetime);

		try
		{
			var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtSecurityToken ||
				!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}

			return principal;
		}
		catch (SecurityTokenException)
		{
			return null;
		}
	}

	public ClaimsPrincipal? GetPrincipalFromToken(string token)
		=> GetPrincipalFromToken(token, false);

	private static IEnumerable<Claim> GetClaimsForUser(User user)
	{
		return
		[
			new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new Claim(ClaimTypes.Role, UserRole.CLIENT.ToString()),
			new Claim(ClaimTypes.Name, user.Username),
		];
	}

	private static IEnumerable<Claim> GetClaimsForBusiness(Business business)
	{
		return
		[
			new(ClaimsKeys.BusinessId, business.Id.ToString()),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		];
	}

	private TokenValidationParameters GetTokenValidationParameters(bool validateLifetime = true)
	{
		return new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = _jwtSettings.Issuer,
			ValidateAudience = true,
			ValidAudience = _jwtSettings.Audience,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = CreateSigningKey(),
			ValidateLifetime = validateLifetime
		};
	}

	private SymmetricSecurityKey CreateSigningKey()
	{
		return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
	}
}