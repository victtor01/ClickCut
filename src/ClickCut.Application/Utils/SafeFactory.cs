namespace ClickCut.Application.Utils;

using ClickCut.Shared.Utils;

public static class SafeFactory
{
	public static Result<T> TryCreate<T>(Func<T> factoryFunction)
	{
		try
		{
			T instance = factoryFunction();
			return Result<T>.Success(instance);
		}
		catch (ArgumentException ex)
		{
			return Result<T>.Failure(ex.Message);
		}
		catch (InvalidOperationException ex)
		{
			return Result<T>.Failure(ex.Message);
		}
	}

}