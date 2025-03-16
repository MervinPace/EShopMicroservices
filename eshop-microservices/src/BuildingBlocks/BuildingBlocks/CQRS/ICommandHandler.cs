using MediatR;

namespace BuildingBlocks.CQRS
{
    //This is defined for when we have no response. Unit is the null return type for Mediatr
    public interface ICommandHandler<in TCommand>
        : ICommandHandler<TCommand,Unit>
        where TCommand : ICommand<Unit>
    {

    }

    public interface ICommandHandler<in TCommand, TResponse> 
        : IRequestHandler<TCommand,TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
    }
}
