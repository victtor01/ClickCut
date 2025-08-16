using System.Security.Claims;
using ClickCut.Domain.Models;

namespace ClickCut.Application.Ports.In;

public interface IJwtServicePort
{
	string Generate(User usuario);
	string Generate(User usuario, int minutes);
	string Generate(Business usuario);
	string Generate(Business usuario, int minutes);
	ClaimsPrincipal? GetPrincipalFromToken(string token, bool validateLifetime);
	ClaimsPrincipal? GetPrincipalFromToken(string token);
}
