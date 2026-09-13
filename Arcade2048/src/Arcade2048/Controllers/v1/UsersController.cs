namespace Arcade2048.Controllers.v1;

using Arcade2048.Domain.Users;
using Arcade2048.Domain.Users.Dtos;
using Arcade2048.Domain.Users.Features;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("api/v{v:apiVersion}/users")]
[ApiVersion("1.0")]
public sealed class UsersController(IMediator mediator): ControllerBase
{
    /// <summary>
    /// Register a new user and returns access and refresh tokens.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register", Name = "RegisterUser")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserDto request)
    {
        var command = new RegisterUser.Command(request);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// login an user and returns access and refresh tokens
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login", Name = "LoginUser")]
    public async Task<ActionResult<AuthResponseDto>> login([FromBody] LogInUserDto request)
    {
        var command = new LogInUser.Command(request);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// logout an user.
    /// </summary>
    [AllowAnonymous]
    [Authorize]
    [HttpPost("logout", Name = "LogoutUser")]
    public async Task<ActionResult<AuthResponseDto>> Logout()
    {
        var command = new LogOutUser.Command();
        await mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// refresh user access token.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh/token", Name = "RefreshAccessToken")]
    public async Task<ActionResult<AuthResponseDto>> RefreshAccessToken([FromBody] RefreshUserTokenDto request)
    {
        var command = new RefreshUserToken.Command(request);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new User record.
    /// </summary>
    [HttpPost(Name = "AddUser")]
    public async Task<ActionResult<UserDto>> AddUser([FromBody]UserForCreationDto userForCreation)
    {
        var command = new AddUser.Command(userForCreation);
        var commandResponse = await mediator.Send(command);

        return CreatedAtRoute("GetUser",
            new { userId = commandResponse.Id },
            commandResponse);
    }


    /// <summary>
    /// Gets a single User by ID.
    /// </summary>
    [Authorize]
    [HttpGet("{userId:guid}", Name = "GetUser")]
    public async Task<ActionResult<UserDto>> GetUser(Guid userId)
    {
        var query = new GetUser.Query(userId);
        var queryResponse = await mediator.Send(query);
        return Ok(queryResponse);
    }


    /// <summary>
    /// Gets a list of all Users.
    /// </summary>
    [Authorize]
    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers([FromQuery] UserParametersDto userParametersDto)
    {
        var query = new GetUserList.Query(userParametersDto);
        var queryResponse = await mediator.Send(query);

        var paginationMetadata = new
        {
            totalCount = queryResponse.TotalCount,
            pageSize = queryResponse.PageSize,
            currentPageSize = queryResponse.CurrentPageSize,
            currentStartIndex = queryResponse.CurrentStartIndex,
            currentEndIndex = queryResponse.CurrentEndIndex,
            pageNumber = queryResponse.PageNumber,
            totalPages = queryResponse.TotalPages,
            hasPrevious = queryResponse.HasPrevious,
            hasNext = queryResponse.HasNext
        };

        Response.Headers.Append("X-Pagination",
            JsonSerializer.Serialize(paginationMetadata));

        return Ok(queryResponse);
    }


    /// <summary>
    /// Updates an entire existing User.
    /// </summary>
    [HttpPut("{userId:guid}", Name = "UpdateUser")]
    public async Task<IActionResult> UpdateUser(Guid userId, UserForUpdateDto user)
    {
        var command = new UpdateUser.Command(userId, user);
        await mediator.Send(command);
        return NoContent();
    }


    /// <summary>
    /// Deletes an existing User record.
    /// </summary>
    [HttpDelete("{userId:guid}", Name = "DeleteUser")]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        var command = new DeleteUser.Command(userId);
        await mediator.Send(command);
        return NoContent();
    }

    // endpoint marker - do not delete this comment
}
