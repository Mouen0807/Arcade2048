namespace Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;

using AutoBogus;
using Arcade2048.Domain.ExternalLogins;
using Arcade2048.Domain.ExternalLogins.Dtos;

public sealed class FakeExternalLoginForCreationDto : AutoFaker<ExternalLoginForCreationDto>
{
    public FakeExternalLoginForCreationDto()
    {
    }
}