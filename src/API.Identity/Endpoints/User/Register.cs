using API.Identity.Extensions;
using API.Identity.Infrastructure;
using Application.Users.Register;
using MediatR;

namespace API.Identity.Endpoints.User;

internal sealed class Register : IEndpoint
{
    public sealed class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TelegramUserId { get; set; }
        public string TelegramUsername { get; set; }
        public string Localization { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("register", async (RegisterRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                TelegramUserId = request.TelegramUserId,
                TelegramUsername = request.TelegramUsername,
                Localization = request.Localization
            };

            var result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
