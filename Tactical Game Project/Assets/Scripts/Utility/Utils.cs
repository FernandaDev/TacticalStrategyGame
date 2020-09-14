public static class Layers
{
    public const int TILE_LAYERMASK = 1 << 8;
    public const int UNITS_LAYERMASK = 1 << 9;
}

public enum SelectionState
{
    UnitSelection,
    CommandTileSelection
}

public enum CommandType
{
    Default,
    Move,
    Attack,
    Skill,
    Hover,
    Target
}
