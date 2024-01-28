using MediatR;

namespace Application.Queries;

public sealed record GetAllCategoriesQuery(bool TrackChanges) : IRequest<IQueryable<string?>>;