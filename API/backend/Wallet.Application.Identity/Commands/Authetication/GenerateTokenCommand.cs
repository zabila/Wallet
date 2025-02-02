using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.Authentication;

public sealed record GenerateTokenCommand(UserForAuthenticationDto UserForAuthenticationDto, bool PopulateExp) : IRequest<TokenDto>;