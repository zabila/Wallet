using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.Authetication;

public sealed record GenerateTokenCommand(UserForAuthenticationDto UserForAuthenticationDto, bool PopulateExp) : IRequest<TokenDto>;