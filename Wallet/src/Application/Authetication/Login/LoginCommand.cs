using Application.Messaging;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.DTO.Login;

namespace Application.Authetication.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;
