using Microsoft.Extensions.Logging;

namespace Core.Application.Common;

public class ValidatorBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
    private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger = logger;


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("Validating command {CommandType}", typeName);

        var validations = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request)));
        var errors = validations.Where(x => !x.IsValid).SelectMany(result => result.Errors).Select(x => new BussinessValidtion.ValidationError(x.ErrorCode, x.PropertyName, x.ErrorMessage)).ToList();

        if (errors.Count != 0)
        {
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, errors);
            throw new BussinessValidtion(
                $"Command Validation Errors for type {typeof(TRequest).Name}", errors);
        }

        return await next();
    }
}
