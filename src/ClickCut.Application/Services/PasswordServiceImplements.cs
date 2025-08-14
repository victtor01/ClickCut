using ClickCut.Application.Ports.In;
using ClickCut.Domain.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ClickCut.Application.Services;

public class PasswordServiceImplements : IPasswordServicePort
{
	public string Encoder(string password)
		=> BCryptNet.HashPassword(password);

	public bool Verify(string passwordToVerify, Password hash)
		=> BCryptNet.Verify(passwordToVerify.ToString(), hash.ToString());

	public bool Verify(string passwordToVerify, string hash)
		=> BCryptNet.Verify(passwordToVerify.ToString(), hash.ToString());
}