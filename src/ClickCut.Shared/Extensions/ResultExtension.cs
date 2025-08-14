using ClickCut.Shared.Exceptions;
using ClickCut.Shared.Utils;

namespace ClickCut.Shared.Extensions;

public static class ResultExtensions
{
	public static T ThrowIfHasError<T>(this Result<T> result)
	{
		if (result.IsFailure)
			throw new BadRequestException(result.Error);

		return result.Value!;
	}
}