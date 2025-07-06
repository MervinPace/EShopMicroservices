namespace Catalog.API.Products.GetProductsByCategory;

record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;

record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductsByQueryHandler 
(IDocumentSession session, ILogger<GetProductsByQueryHandler> logger)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsByQueryHandler.Handle called by {@Query})", request);
        
        var products = await session.Query<Product>()
            .Where(p=>p.Category.Contains(request.Category))
            .ToListAsync(cancellationToken);
        
        return new GetProductsByCategoryResult(products);
    }
    
}