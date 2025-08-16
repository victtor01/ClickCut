using ClickCut.Api.Dtos.Business;
using ClickCut.Api.Mappers;
using ClickCut.Application.Commands;
using ClickCut.Application.Ports.In;
using ClickCut.Application.Services;
using ClickCut.Domain.Models;
using ClickCut.Infra.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClickCut.Api.Controllers;

[Authorize]
[ApiController]
[Route("/business")]
public class BusinessController(IBusinessServicePort businessServicePort) : ControllerBase
{
	private readonly IBusinessServicePort _businessServicePort = businessServicePort;

	[HttpPost]
	public async Task<ActionResult<BusinessResponse>> Create([FromBody] CreateBusinessRequest createBusinessRequest)
	{
		Guid userId = HttpContext.User.GetUserId();

		Business business = await _businessServicePort.CreateAsync(new CreateBusinessCommand(
			Name: createBusinessRequest.Name,
			Password: createBusinessRequest.Password,
			OwnerId: userId
		));

		return CreatedAtAction(nameof(FindById), new { businessId = business.Id }, business.ToResponse());
	}

	[HttpGet("all")]
	public async Task<ActionResult<List<BusinessResponse>>> FindAll()
	{
		Guid userId = HttpContext.User.GetUserId();

		List<Business> businesses = await _businessServicePort.FindByUserAndMembersAsync(userId);

		return Ok(businesses.Select(u => u.ToResponse()).ToList());
	}

	[HttpGet("{businessId}")]
	public IActionResult FindById(Guid businessId)
	{
		return Ok(businessId);
	}
}