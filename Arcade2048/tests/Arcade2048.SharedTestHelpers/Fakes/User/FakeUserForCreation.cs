namespace Arcade2048.SharedTestHelpers.Fakes.User;

using AutoBogus;
using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.Models;

public sealed class FakeUserForCreation : AutoFaker<UserForCreation>
{
    public FakeUserForCreation()
    {
    }
}