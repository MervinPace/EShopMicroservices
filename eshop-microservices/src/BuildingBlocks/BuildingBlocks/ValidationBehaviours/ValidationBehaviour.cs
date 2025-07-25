using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.ValidationBehaviours;

// ValidationBehaviour is a pipeline behavior for MediatR.
// It intercepts any command (ICommand) before it reaches the handler,
// and ensures that all associated FluentValidation validators run first.
public class ValidationBehaviour<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators) // This is a primary constructor introduced in C# 12.We are passing an IEnumerable to pass all the validation rules
    : IPipelineBehavior<TRequest, TResponse>       // This class implements MediatR's pipeline behavior.
    where TRequest : ICommand<TResponse>            // TRequest must be a command (ICommand). CRUD operations in this case and not read(query)
{
    // This method runs during the request pipeline.
    // It's invoked before the actual command handler executes.
    public async Task<TResponse> Handle(
        TRequest request,                         // The incoming request/command.
        RequestHandlerDelegate<TResponse> next,   // Delegate to call the next step (typically the handler).
        CancellationToken cancellationToken)      // Allows operation cancellation.
    {
        // Create a FluentValidation context for the current request.
        var context = new ValidationContext<TRequest>(request);

        // Run all validators asynchronously on the request context.
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Gather all validation failures from the results.
        var failures = validationResults
            .Where(r => r.Errors.Any())           // Keep only those with errors.
            .SelectMany(r => r.Errors)            // Flatten the list of errors.
            .ToList();

        // If any validation errors were found, throw a ValidationException
        // which will likely stop the request from proceeding further.
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        // If validation passed, proceed with the next step in the pipeline.
        return await next();
    }
}
