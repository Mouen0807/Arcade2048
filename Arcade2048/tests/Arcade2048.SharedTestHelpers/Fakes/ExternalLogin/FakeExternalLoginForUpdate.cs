namespace Arcade2048.SharedTestHelpers.Fakes.ExternalLogin;

using AutoBogus;
using Arcade2048.Domain.ExternalLogins;
using Arcade2048.Domain.ExternalLogins.Models;

public sealed class FakeExternalLoginForUpdate : AutoFaker<ExternalLoginForUpdate>
{
    public FakeExternalLoginForUpdate()
    {
    }
}