using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.User;

public sealed record UpdateUserCommand(UpdateUserDto UpdateUserDto) : IRequest<IdentityResult>;