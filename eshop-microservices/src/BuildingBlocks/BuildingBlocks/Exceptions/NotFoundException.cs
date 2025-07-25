namespace BuildingBlocks.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base("Message")
    {
    }

    public NotFoundException(string name, object key) : base($"Entity {name} with key {key} not found")
    {
    }
}