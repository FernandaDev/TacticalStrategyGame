using System;

namespace FernandaDev
{
    public interface ICommandController
    {
        event Action OnCommandAnimationEnd;
        bool IsSelectionViable(Tile selectedTile);
        Unit myUnit { get; }
        CommandType commandType { get; }
    }
}