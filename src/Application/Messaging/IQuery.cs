using MediatR;
using SharedKernel;

namespace Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
