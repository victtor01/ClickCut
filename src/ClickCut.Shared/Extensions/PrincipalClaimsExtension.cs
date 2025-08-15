using System.Security.Claims;

namespace ClickCut.Infra.Extensions // Ou o namespace que fizer mais sentido no seu projeto
{
	public static class ClaimsPrincipalExtensions
	{
		public static Guid GetUserId(this ClaimsPrincipal principal)
		{
			if (principal == null)
			{
				throw new ArgumentNullException(nameof(principal));
			}

			var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (Guid.TryParse(userIdClaim, out var userId))
			{
				return userId;
			}

			return Guid.Empty;
		}
	}
}