using BattleshipLogic.Interfaces;

namespace BattleshipLogic.Interfaces
{
    public interface IInputValidator
    {
        BattleViewModel Validate(string input);
    }
}