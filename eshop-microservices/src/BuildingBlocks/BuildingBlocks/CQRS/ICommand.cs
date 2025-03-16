// Using MediatR library which provides the base IRequest interface
using MediatR;

namespace BuildingBlocks.CQRS
{
    // This interface is for commands that don't return any value
    // Unit is MediatR's representation of void - it's used when no return value is needed
    public interface ICommand : ICommand<Unit>
    {
    }

    // This interface is for commands that return a value of type TResponse
    // The 'out' keyword makes TResponse covariant, meaning you can use a more derived type than originally specified
    // It inherits from MediatR's IRequest<TResponse>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}