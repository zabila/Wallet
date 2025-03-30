namespace SharedKernel.Exceptions;

public sealed class ValidationAppException : Exception
{
    public ValidationAppException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred")
    {
        Errors = errors;
    }

    public ValidationAppException()
    {
    }

    public ValidationAppException(string message)
        : base(message)
    {
    }

    public ValidationAppException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public IReadOnlyDictionary<string, string[]>? Errors { get; }
}
