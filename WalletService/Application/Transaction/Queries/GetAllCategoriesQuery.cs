using MediatR;

namespace Application.Transaction.Queries;

public sealed record GetAllCategoriesQuery(bool TrackChanges) : IRequest<IQueryable<string?>>;