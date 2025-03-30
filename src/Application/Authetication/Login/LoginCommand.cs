using Application.Messaging;
using SharedKernel.DTO.Login;

namespace Application.Authetication.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;
