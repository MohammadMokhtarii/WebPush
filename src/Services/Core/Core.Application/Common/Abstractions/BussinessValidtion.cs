namespace Core.Application.Common;

public class BussinessValidtion(string message, IEnumerable<BussinessValidtion.ValidationError> errors) : Exception(message)
{
    public IEnumerable<ValidationError> Errors { get; init; } = errors;
    public record ValidationError(string Code, string PropertyName, string Message);
}