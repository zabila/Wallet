using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.Authetication;

public sealed record LoginCommand(UserForAuthenticationDto UserForAuthenticationDto) : IRequest<SignInResult>;