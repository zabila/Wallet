using MediatR;
using Wallet.Shared.DataTransferObjects;

namespace Wallet.Application.Identity.Commands.User;

public sealed record AssignRoleCommand(AssignRoleDto AssignRoleDto) : IRequest;