using BattleshipLogic.Interfaces;
using System;
using System.Linq;

namespace BattleshipLogic
{
    public class InputValidator : IInputValidator
    {
        public IGameResources GameResources { get; private set; }
        public IGameOptions GameOptions { get; private set; }
        public BattleViewModel vm { get; private set; }
        public IGameRepository GameRepository { get; private set; }
        public InputValidator(IGameResources resx, IGameRepository repo, IGameOptions opt)
        {
            GameResources = resx;
            GameRepository = repo;
            GameOptions = opt;            
        }
        public BattleViewModel Validate(string input)
        {
            vm = new BattleViewModel();
            vm.InputIsValid = true;

            if (string.IsNullOrWhiteSpace(input) || input.Length != 2)
            {
                vm.Messages.Add(GameResources.MESSAGE_INPUT_ERROR_LENGTH);
                vm.InputIsValid = false;
                return vm;
            }

            if (!Char.IsLetter(input[0]) || !Char.IsDigit(input[1]))
            {
                vm.Messages.Add(GameResources.MESSAGE_INPUT_ERROR_CHAR);
                vm.InputIsValid = false;
                return vm;
            }

            Location attLoc = LocationTranslatior.TranslateInputToGameLocation(input);

            if (attLoc.xPos > GameOptions.BoardSize - 1 || attLoc.yPos > GameOptions.BoardSize - 1)
            {
                vm.Messages.Add(String.Format(GameResources.MESSAGE_INPUT_ERROR_OUT_OF_RANGE, GameOptions.BoardSize));
                vm.Messages.Add(GameResources.MESSAGE_ATTACK);
                vm.InputIsValid = false;
            }
            else
            {
                if (LocationHasBeenAttacked(attLoc.xPos, attLoc.yPos))
                {
                    vm.Messages.Add(GameResources.MESSAGE_ALREADY_ATTACKED);
                    vm.AttackedLocations = GameRepository.AttackedLocations;
                    vm.ShipsSunk = GameRepository.Ships.Where(s => s.Hits == s.Size).ToList().Count;
                    vm.InputIsValid = false;
                }
                else
                {
                    GameRepository.AttackedLocations.Add(attLoc);
                    vm.AttackLocation = attLoc;
                    vm.AttackedLocations = GameRepository.AttackedLocations;
                    vm.ShipsSunk = GameRepository.Ships.Where(s => s.Hits == s.Size).ToList().Count;
                }
            }

            return vm;
        }        

        private bool LocationHasBeenAttacked(int x, int y)
        {
            return GameRepository.AttackedLocations?.Where(s => s.xPos == x && s.yPos == y).FirstOrDefault() != null;
        }
    }
}
