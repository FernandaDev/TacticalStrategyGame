namespace FernandaDev
{
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
        Move,
        Attack,
        Skill
    }

    public enum MaterialType
    {
        Default,
        Move,
        Attack,
        Skill,
        Hover,
        Target
    }

    public enum UnitJob
    {
        Unit
    }
}
