using BattleshipLogic.Interfaces;

namespace BattleshipClient
{
    public class GameResources : IGameResources
    {
        public string MESSAGE_ATTACK { get; } = $"Enter attack coordinates [A-J][0-9]: ";
        public string MESSAGE_HIT_SHIP { get; } = $"You hit a ship.";
        public string MESSAGE_SUNK_SHIP { get; } = $"You sunk the computer's ship.";
        public string MESSAGE_MISSED_SHIP { get; } = $"MISS - Enter attack coordinates [A-J][0-9]: ";
        public string MESSAGE_ALREADY_ATTACKED { get; } = $"You already attacked this position, attack something else.";
        public string MESSAGE_INPUT_ERROR_LENGTH { get; } = $"INPUT ERROR: Enter attack coordinates [A-J][0-9], example A0:  ";
        public string MESSAGE_INPUT_ERROR_CHAR { get; } = $"INPUT ERROR: Enter attack coordinates [A-J][0-9], example A0:";
        public string MESSAGE_INPUT_ERROR_OUT_OF_RANGE { get; } = "INPUT OUTSIDE GAME BOARD: Board size is {0}";
        public string MESSAGE_GAME_OVER_COMPUTER_WIN { get; } = "GAME OVER - COMPUTER WINS";
        public string MESSAGE_GAME_OVER_PLAYER_WIN { get; } = "GAME OVE - PLAYER WINS";
        public string APPLICATION_TITLE { get; } = "Battleships";
        public string MESSAGE_WELCOME { get; } = "New game, the computer has placed {0} ships.";
        public string MESSAGE_PLAY_AGAIN { get; } = "Play again (Y/N)";
        public string MESSAGE_USER_EXIT { get; } = "GAME OVE - PLAYER QUITS";
    }
}
