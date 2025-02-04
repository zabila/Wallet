using MediatR;
using Wallet.SharedKernel.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.User;

public sealed record AssignRoleCommand(AssignRoleDto AssignRoleDto) : IRequest;