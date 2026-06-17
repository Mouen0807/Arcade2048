namespace Arcade2048.Domain.ExternalLogins.Dtos;

using Destructurama.Attributed;

public sealed record ExternalLoginForUpdateDto
{
    public Guid UserId { get; set; }
    public string Provider { get; set; }
    public string ExternalId { get; set; }
    public string? Email { get; set; }
    public DateTime LinkedAt { get; set; }
}
