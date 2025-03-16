// Using MediatR library for mediator pattern implementation
using MediatR;

namespace BuildingBlocks.CQRS
{
    // Interface for queries that will return data
    // Internal - only accessible within the same assembly
    // TResponse - the type of data that will be returned
    // out - makes TResponse covariant (allowing more derived types)
    // IRequest<TResponse> - inherits from MediatR's request interface
    // where TResponse : notnull - ensures TResponse cannot be null
    public interface IQuery<out TResponse> : IRequest<TResponse>
        where TResponse : notnull
    {
    }
}