using System.ComponentModel.DataAnnotations;
using Application.Messaging;
using MediatR;

namespace Application.Users.Update;

public sealed record UpdateUserCommand : ICommand
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int TelegramUserId { get; set; }
    public string TelegramUsername { get; set; }
    public string Localization { get; set; }
}
