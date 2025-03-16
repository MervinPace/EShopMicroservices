// Import the MediatR library which provides the mediator pattern implementation
using BuildingBlocks.CQRS;
using Catalog.API.Models;
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
    public record CreateProductResult(Guid id);

    // Command Handler: This is where the actual business logic for creating a product will go
    // IRequestHandler<TRequest, TResponse> interface requires implementing Handle method
    // TRequest = CreateProductCommand (input)
    // TResponse = CreateProductResult (output)
    internal class CreateProductCommandHandler 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        // Handle Method: This is where you implement the actual product creation logic
        // Parameters:
        //   - request: Contains all the product information from CreateProductCommand
        //   - cancellationToken: Allows cancellation of long-running operations
        // Returns: Task containing the CreateProductResult (the new product's ID)
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };
            // Currently not implemented - this is where you would:
            // 1. Validate the input
            // 2. Create product in database
            // 3. Return the new product's ID
            // 4. Handle any errors that might occur
            return new  (Guid.NewGuid());
        }
    }
}