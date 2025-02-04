using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.User;

public sealed record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequest<IdentityResult>;