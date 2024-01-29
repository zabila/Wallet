using Application.Queries;
using Contracts;
using MediatR;

namespace Application.Handlers;

internal sealed class GetAllCategoriesQueryHandler(IRepositoryManager repository, ILoggerManager logger) : IRequestHandler<GetAllCategoriesQuery, IQueryable<string?>>
{
    public Task<IQueryable<string?>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        logger.LogDebug($"GetAllCategoriesQueryHandler: Getting all categories");

        var categories = repository.Transaction.GetAllCategories(request.TrackChanges);
        return Task.FromResult(categories);
    }
}