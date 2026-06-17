namespace Arcade2048.SharedTestHelpers.Fakes.User;

using AutoBogus;
using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.Dtos;

public sealed class FakeUserForUpdateDto : AutoFaker<UserForUpdateDto>
{
    public FakeUserForUpdateDto()
    {
    }
}