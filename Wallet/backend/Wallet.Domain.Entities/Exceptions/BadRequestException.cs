namespace Wallet.Domain.Entities.Exceptions;

public abstract class BadRequestException(string message) : Exception(message) {
}