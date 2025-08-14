using ClickCut.Domain.Models;

namespace ClickCut.Application.Ports.In;

public interface IPasswordServicePort
{
	public string Encoder(string password);
	public bool Verify(string pass, Password hash);
	public bool Verify(string pass, string hash);
}