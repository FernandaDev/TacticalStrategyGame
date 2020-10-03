namespace FernandaDev
{
    public class AttackCommand : ICommand
    {
        public ICommandController commandController { get; private set; }
        AttackController _attackController;
        Tile _targetTile;
        Unit _attackedUnit;

        public AttackCommand(AttackController attackController, Tile targetTile)
        {
            _targetTile = targetTile;
            _attackedUnit = targetTile.UnitOverTile;
            _attackController = attackController;
            commandController = attackController;
        }

        public void Execute()
        {
            _attackController.StartAttack(_targetTile);
        }

        public void Undo()
        {
            _attackController.CancelAttack(_attackedUnit);
        }
    }
}