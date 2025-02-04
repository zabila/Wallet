namespace Wallet.Domain.Entities.Exceptions;

public class RoleNotFoundException : NotFoundException
{
    public RoleNotFoundException(string role) : base($"The role with name: {role} doesn't exist.")
    {
    }

    public RoleNotFoundException() : base("The role doesn't exist.")
    {
    }

    public RoleNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
