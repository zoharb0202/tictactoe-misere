namespace TicTacToeMisere.GameLogic
{
    public enum eCellSign
    {
        Empty = 0,
        X = 1,
        O = 2,
    }

    public enum ePlayerType
    {
        Human,
        Computer,
    }

    public enum eGameStatus
    {
        InProgress,
        Win,
        Tie,
    }
}
