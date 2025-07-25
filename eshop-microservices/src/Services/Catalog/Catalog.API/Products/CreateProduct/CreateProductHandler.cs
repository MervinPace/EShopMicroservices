// Import the MediatR library which provides the mediator pattern implementation
using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using MediatR;
using System.Windows.Input;

namespace Catalog.API.Products.CreateProduct
{
    // Command Record: Represents the request to create a new product
    // This is like a Data Transfer Object (DTO) that carries all the information needed to create a product
    // IRequest<T> interface indicates this is a command that will return a CreateProductResult
    public record CreateProductCommand(
        string Name,              // Name of the product
        List<string> Category,    // List of categories the product belongs to
        string Description,       // Product description
        string ImageFile,         // Path or reference to product image
        decimal Price            // Price of the product
    ) : ICommand<CreateProductResult>;

    // Result Record: Represents what will be returned after creating the product
    // In this case, we only return the newly created product's unique identifier
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
                RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
                RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
                RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }

    // Command Handler: This is where the actual business logic for creating a product will go
    // IRequestHandler<TRequest, TResponse> interface requires implementing Handle method
    // TRequest = CreateProductCommand (input)
    // TResponse = CreateProductResult (output)
    internal class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        // Handle Method: This is where you implement the actual product creation logic
        // Parameters:
        //   - request: Contains all the product information from CreateProductCommand
        //   - cancellationToken: Allows cancellation of long-running operations
        // Returns: Task containing the CreateProductResult (the new product's ID)
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            //This is now configured to use IPipeline behaviour from buildingbllocks library
            //// var result = await validator.ValidateAsync(command, cancellationToken);
            // var errors = result.Errors.Select(x=>x.ErrorMessage).ToList();
            //
            // if (errors.Any())
            // {
            //     throw new ValidationException(errors.FirstOrDefault());
            // }
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            //return result
            return new CreateProductResult(product.Id);
        }
    }
}