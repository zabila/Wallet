using API.Identity.Extensions;
using API.Identity.Infrastructure;
using Application.Users.Update;
using MediatR;

namespace API.Identity.Endpoints.User;

internal sealed class Update : IEndpoint
{
    public sealed class UpdateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TelegramUserId { get; set; }
        public string TelegramUsername { get; set; }
        public string Localization { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("update/{userId:guid}", async (Guid userId, UpdateRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new UpdateUserCommand
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                TelegramUserId = request.TelegramUserId,
                TelegramUsername = request.TelegramUsername,
                Localization = request.Localization
            };

            var result = await sender.Send(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
