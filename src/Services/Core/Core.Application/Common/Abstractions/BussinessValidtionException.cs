namespace Core.Application.Common;

public sealed class BussinessValidtionException(string message, IEnumerable<BussinessValidtionException.ValidationError> errors) : Exception(message)
{
    public IEnumerable<ValidationError> Errors { get; init; } = errors;
    public record ValidationError(string Code, string PropertyName, string Message);
}