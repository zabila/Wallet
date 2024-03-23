using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.Authetication;

public sealed record LoginCommand(UserForAuthenticationDto UserForAuthenticationDto) : IRequest<SignInResult>;