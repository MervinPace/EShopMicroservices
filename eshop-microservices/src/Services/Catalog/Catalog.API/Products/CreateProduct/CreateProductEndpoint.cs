namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(
        string Name,              // Name of the product
        List<string> Category,    // List of categories the product belongs to
        string Description,       // Product description
        string ImageFile,         // Path or reference to product image
        decimal Price            // Price of the product
        );

    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) => //anonymous method
                {
                    //map request to command
                    var command = request.Adapt<CreateProductCommand>();
                    //trigger mediatr handler class 
                    var result = await sender.Send(command);
                    //map back the result to response
                    var response = result.Adapt<CreateProductResponse>();
                    //return response
                    return Results.Created($"/products/{response.Id}", response);

                })//carter extensions to methods to configure http method
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
