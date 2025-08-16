using System.ComponentModel.DataAnnotations;

namespace ClickCut.Api.Dtos.Auth;

public record AuthBusinessRequest([Required] Guid BusinessId);