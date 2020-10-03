namespace FernandaDev
{
    public interface ICommand
    {
        ICommandController commandController { get; }
        void Execute();
        void Undo();
    }
}