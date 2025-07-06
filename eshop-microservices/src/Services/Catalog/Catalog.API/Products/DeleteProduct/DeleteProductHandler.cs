namespace Catalog.API.Products.DeleteProduct;

record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler
(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
: ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling DeleteProductCommand for product with ID: {Id}", command.Id);
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with ID: {Id} not found", command.Id);
            return new DeleteProductResult(false);
        }
        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Product with ID: {Id} deleted successfully", command.Id);
        return new DeleteProductResult(true);
    }
}