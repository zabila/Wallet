using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Application.Messaging;
using Domain.Transactions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel.DTO.Login;

namespace Application.Users.Register;

public sealed record RegisterUserCommand : ICommand<TokenResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TelegramUserId { get; set; }
    public string TelegramUsername { get; set; }
    public string Localization { get; set; }
}
