using ClickCut.Domain.Models;
using ClickCut.Shared.Exceptions;

namespace ClickCut.Api.Extensions;

public static class SessionExtensions
{
	private const string SessionKey = "PointSale_Session";

	public static void SetBusinessSession(this HttpContext context, BusinessSession session)
	{
		context.Items[SessionKey] = session;
	}

	public static T GetSession<T>(this HttpContext context) where T : BusinessSession
	{
		if (context.Items[SessionKey] is T session)
			return session;

		throw new BadRequestException($"Sessão não encontrada ou o tipo da sessão não é '{typeof(T).Name}'.");
	}

	public static BusinessSession GetRequiredSession(this HttpContext context)
	{
		if (context.Items[SessionKey] is BusinessSession session)
			return session;

		throw new BadRequestException("Nenhuma sessão válida foi encontrada na requisição.");
	}
}