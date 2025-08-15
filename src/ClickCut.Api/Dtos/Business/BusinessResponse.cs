namespace ClickCut.Api.Dtos.Business;

public record BusinessResponse(
	Guid Id,
	string Name,
	bool HasPassword
);