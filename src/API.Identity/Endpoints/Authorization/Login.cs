using API.Identity.Extensions;
using API.Identity.Infrastructure;
using Application.Authetication.Login;
using MediatR;

namespace API.Identity.Endpoints.Authorization;

internal sealed class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("login", async (LoginRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .WithTags(Tags.Authorization)
            .RequireAuthorization();
    }

    public sealed record LoginRequest(string Email, string Password);
}
