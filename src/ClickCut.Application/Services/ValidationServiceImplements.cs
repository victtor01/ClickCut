using ClickCut.Application.Ports.In;
using ClickCut.Shared.Exceptions;

namespace ClickCut.Application.Services;

public class ValidationServiceImplements : IValidationServicePort
{
	private readonly Dictionary<string, List<string>> _errors = [];

	public IReadOnlyDictionary<string, IReadOnlyList<string>> Errors =>
		_errors.ToDictionary(kvp => kvp.Key, kvp => (IReadOnlyList<string>)kvp.Value.AsReadOnly());

	public IValidationServicePort Check(bool isInvalid, string key, string message)
	{
		if (isInvalid)
			AddError(key, message);

		return this;
	}

	public void AddError(string key, string message)
	{
		if (!_errors.ContainsKey(key))
			_errors[key] = [];

		_errors[key].Add(message);
	}

	public void ThrowIfHasErrors()
	{
		if (_errors.Any())
		{
			var errorsDictionary = _errors.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray());
			throw new BadRequestException("Um ou mais erros de validação ocorreram.", errorsDictionary);
		}
	}
}