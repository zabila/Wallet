using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Commands;

public sealed record UpdateUserCommand(UpdateUserDto UpdateUserDto) : IRequest<IdentityResult>;