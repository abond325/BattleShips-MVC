namespace BattleshipLogic.Interfaces
{
    public interface IGameResources
    {
        string MESSAGE_ALREADY_ATTACKED { get; }
        string MESSAGE_ATTACK { get; }
        string MESSAGE_HIT_SHIP { get; }
        string MESSAGE_MISSED_SHIP { get; }
        string MESSAGE_SUNK_SHIP { get; }
        string MESSAGE_INPUT_ERROR_LENGTH { get; }
        string MESSAGE_INPUT_ERROR_CHAR { get; }
        string MESSAGE_INPUT_ERROR_OUT_OF_RANGE { get; }
        string MESSAGE_GAME_OVER_COMPUTER_WIN { get; }
        string MESSAGE_GAME_OVER_PLAYER_WIN { get; }
        string APPLICATION_TITLE {get;}
        string MESSAGE_WELCOME { get; }
        string MESSAGE_PLAY_AGAIN { get; }
        string MESSAGE_USER_EXIT { get; }
    }
}