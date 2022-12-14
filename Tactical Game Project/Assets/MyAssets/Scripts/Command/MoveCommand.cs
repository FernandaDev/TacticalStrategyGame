namespace FernandaDev
{
    public class MoveCommand : ICommand
    {
        public ICommandController commandController { get; private set; }
        private MovementController _moveController;
        private Tile _oldTile;
        private Tile _newTile;

        public MoveCommand(MovementController moveController, Tile oldTile, Tile newTile)
        {
            _moveController = moveController;
            commandController = moveController;
            _oldTile = oldTile;
            _newTile = newTile;
        }

        public void Execute()
        {
            _moveController.StartMove(_newTile);
        }

        public void Undo()
        {
            _moveController.MoveDirectlyTo(_oldTile);
        }
    }
}