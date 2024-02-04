namespace Wallet.Domain.Entities.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);