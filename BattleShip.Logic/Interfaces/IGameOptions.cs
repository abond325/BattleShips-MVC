namespace BattleshipLogic.Interfaces
{
    public interface IGameOptions
    {
        int BoardSize { get; }
        int NumberOfBattleships { get; }
        int NumberOfDestroyers { get; }
    }
}