using ClickCut.Shared.Exceptions;

namespace ClickCut.Application.Ports.In;

public interface IValidationServicePort
{
	public void AddError(string key, string message);
	public void ThrowIfHasErrors();
	public IValidationServicePort Check(bool isInvalid, string key, string message);
}