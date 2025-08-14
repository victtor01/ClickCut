namespace ClickCut.Shared.Utils;

public class Result<T>
{
	public T? Value { get; }
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;
	public string Error { get; } = null!;

	private Result(T value)
	{
		IsSuccess = true;
		Value = value;
		Error = string.Empty;
	}

	private Result(string error)
	{
		IsSuccess = false;
		Value = default;
		Error = error;
	}

	public static Result<T> Success(T value) => new(value);
	public static Result<T> Failure(string error) => new(error);
}