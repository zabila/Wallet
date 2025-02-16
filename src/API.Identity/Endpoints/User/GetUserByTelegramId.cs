using API.Identity.Extensions;
using API.Identity.Infrastructure;
using Application.Users.GetUserByTelegramId;
using MediatR;

namespace API.Identity.Endpoints.User;

internal sealed class GetUserByTelegramId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("telegram/{telegrmId:long}", async (long telegrmId, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetUserByTelegramIdQuery(telegrmId), cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
