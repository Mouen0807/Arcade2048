namespace Arcade2048.Domain.ExternalLogins.Features;

using Arcade2048.Databases;
using Arcade2048.Domain.ExternalLogins;
using Arcade2048.Domain.ExternalLogins.Dtos;
using Arcade2048.Domain.ExternalLogins.Models;
using Arcade2048.Services;
using Arcade2048.Exceptions;
using Mappings;
using MediatR;

public static class AddExternalLogin
{
    public sealed record Command(ExternalLoginForCreationDto ExternalLoginToAdd) : IRequest<ExternalLoginDto>;

    public sealed class Handler(Arcade2048DbContext dbContext)
        : IRequestHandler<Command, ExternalLoginDto>
    {
        public async Task<ExternalLoginDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var externalLoginToAdd = request.ExternalLoginToAdd.ToExternalLoginForCreation();
            var externalLogin = ExternalLogin.Create(externalLoginToAdd);

            await dbContext.ExternalLogins.AddAsync(externalLogin, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return externalLogin.ToExternalLoginDto();
        }
    }
}