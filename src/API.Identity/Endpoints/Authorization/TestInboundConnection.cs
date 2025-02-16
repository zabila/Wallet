using API.Identity.Extensions;
using API.Identity.Infrastructure;
using MediatR;
using SharedKernel;

namespace API.Identity.Endpoints.Authorization;
internal sealed class TestInboundConnection : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("test", (ISender sender, CancellationToken cancellationToken) =>
        {
            Result<bool> result = true;
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Authorization)
        .RequireAuthorization();
    }
}
