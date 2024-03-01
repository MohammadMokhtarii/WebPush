using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Services.Common;

public record Result()
{
    protected Result(Error error) : this()
    {
        IsSucess = false;
        Error = error;
    }

    public bool IsSucess { get; init; } = true;

    public Error Error { get; init; } = Error.None;
    public bool IsFailure => !IsSucess;

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);

    public static Result<TResult> Success<TResult>(TResult data) => Result<TResult>.Success(data);
    public static Result<TResult> Failure<TResult>(Error error) => Result<TResult>.Failure(error);

    public static implicit operator Result(Error error)
        => Failure(error);

}
public record Result<TResult> : Result
{
    private Result(TResult data) => this.Data = data;
    private Result(Error error) : base(error) { }

    public TResult Data { get; private set; }

    public static Result<TResult> Success(TResult data) => new(data);
    public static new Result<TResult> Failure(Error error) => new(error);

    public static implicit operator Result<TResult>(TResult result)
        => Success<TResult>(result);

    public static implicit operator Result<TResult>(Error error)
        => Failure<TResult>(error);
}


public record Error
{

    public readonly static Error None = new(string.Empty, string.Empty, default);

    private Error(string code, string message, ErrorType errorType)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
    }

    public string Code { get; init; }
    [JsonIgnore]
    public ErrorType ErrorType { get; init; }
    public string Message { get; init; }

    public static Error Validation(string code, string message)
        => new(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound);

    public static Error Exception(string code, string message)
        => new(code, message, ErrorType.ServerError);

    public override string ToString()
    {
        return $"{ErrorType}-{Code}:{Message}";
    }
}
public enum ErrorType
{
    [Description("Bad Request")]
    Validation = 400,

    [Description("Not Found")]
    NotFound = 404,

    [Description("Internal Server Error")]
    ServerError = 500
}