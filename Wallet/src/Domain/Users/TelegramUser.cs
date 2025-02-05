using System;
using System.Collections.Generic;
using System.Linq;
using SharedKernel;

namespace Domain.Users;

public sealed class TelegramUser : Entity
{
    public Guid Id { get; set; }
    public long TelegramUserId { get; set; }
    public string TelegramUsername { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
}
