namespace Map
{
    public enum Rarity
    {
        Common,
        UnCommon,
        Rare,
        Epic,
    }

    public enum Category
    {
        Weapon_Type1,
        Weapon_Type2,
        Item,
        Trap,
        Wall,
    }

    public enum Direction
    {
        Forward,
        Right,
        Backward,
        Left,
    }
}

namespace AI
{
    public enum AliveState
    {
        Non,
        Alive,
        Dead
    }
    public enum ThinkState
    {
        Non,
        Attack,
        Move,
        CantMove,
        CollisionPredict
    }

    public enum MoveState
    {
        Step,
        Warp,
    }
}

namespace Player
{

}

namespace Game
{
    public enum GameModeState
    {
        Non,
        Tutorial,
        Local,
        Multi
    }

    public enum GameState
    {
        Non,
        Initialize,
        DecidedTheOrder,
        Battle,
        GameSet,
        Finalize,
    }

    public enum BattleState
    {
        Non,
        Initialize,
        Place,
        TurnEnd,
        AIAction,
        Finalize,
        GameSet,
    }
}