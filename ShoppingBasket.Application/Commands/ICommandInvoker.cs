namespace ShoppingBasket.Application.Commands
{
    public interface ICommandInvoker
    {
        void ExecuteCommand(ICommand command);
    }
}
