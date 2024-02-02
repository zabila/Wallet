namespace Entities.Exceptions;

public class AccountAlreadyExistsBadRequestException(string name) : BadRequestException($"Account with name {name} already exists.");