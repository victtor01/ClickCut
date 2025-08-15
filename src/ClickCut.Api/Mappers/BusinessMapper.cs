using ClickCut.Api.Dtos.Business;
using ClickCut.Domain.Models;

namespace ClickCut.Api.Mappers;

public static class BusinessMapper
{
	public static BusinessResponse ToResponse(this Business business)
	{
		return new BusinessResponse(
			Id: business.Id,
			Name: business.Name,
			HasPassword: string.IsNullOrWhiteSpace(business.Password?.ToString())
		);
	}
}