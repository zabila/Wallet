namespace Wallet.Domain.Entities.Exceptions;

public class RoleNotFoundException(string role) : NotFoundException($"The role with name: {role} doesn't exist.") {

}