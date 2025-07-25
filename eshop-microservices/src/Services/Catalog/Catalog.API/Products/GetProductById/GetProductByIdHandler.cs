namespace Catalog.API.Products.GetProducts.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) 
       : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
       public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, 
              CancellationToken cancellationToken)
       {
              logger.LogInformation("GetProductByIdQueryHandler.Handle called by {@Query})", query);
              
              var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
              
              if (product == null)
              {
                     throw new ProductNotFoundException(query.Id);
              }
              
              return new GetProductByIdResult(product);
       }
}