using Application.Queries;
using Contracts;
using MediatR;

namespace Application.Handlers;

internal sealed class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IQueryable<string?>>
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public GetAllCategoriesQueryHandler(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public Task<IQueryable<string?>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"GetAllCategoriesQueryHandler: Getting all categories");

        var categories = _repository.Transaction.GetAllCategories(request.TrackChanges);
        return Task.FromResult(categories);
    }
}