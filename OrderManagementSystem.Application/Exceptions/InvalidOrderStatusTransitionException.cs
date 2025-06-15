namespace OrderManagementSystem.Application.Exceptions;

public class InvalidOrderStatusTransitionException : Exception
{
    public InvalidOrderStatusTransitionException(string message) : base(message)
    {
    }
}