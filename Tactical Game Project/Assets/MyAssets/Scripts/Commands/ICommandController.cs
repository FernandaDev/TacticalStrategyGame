using System;

public interface ICommandController
{
    //event Action OnCommandStart;
    event Action OnCommandAnimationEnd;

    bool IsSelectionViable(Tile selectedTile);
}
