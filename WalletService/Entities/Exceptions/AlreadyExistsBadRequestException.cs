namespace Entities.Exceptions;

public class AlreadyExistsBadRequestException(string name) : BadRequestException($"Entity with name {name} already exists.");