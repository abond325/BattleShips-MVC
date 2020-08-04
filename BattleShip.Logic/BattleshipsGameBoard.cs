using BattleshipLogic.Interfaces;
using System;
using System.Linq;

namespace BattleshipLogic
{
    public class BattleshipsGameBoard : Entity
    {
        public IGameResources GameResources { get; private set; }
        public IGameOptions GameOptions { get; private set; }
        public IGameRepository GameRepository { get; private set; }
        public IInputValidator InputValidator { get; set; }
        public bool PlayAgain { get; private set; } = true;
        public string Title { get { return GameResources.APPLICATION_TITLE; } }

        public BattleshipsGameBoard(IGameResources resx, IGameOptions opt, IGameRepository repo)
        {
            GameResources = resx;
            GameOptions = opt;
            GameRepository = repo;
            InputValidator = new InputValidator(resx, repo, opt);
        }

        public BattleViewModel LaunchAttack(string input)
        {
            BattleViewModel vm = InputValidator.Validate(input);

            while (!vm.GameOver)
            {
                if (!vm.InputIsValid)
                    return vm;

                return GetAttackResult(vm);
            }

            while(!(input.ToUpper() == "Y") || !(input.ToUpper() == "N"))
            {
                if(input.ToUpper() == "Y")
                {
                    NewGame();
                    return vm;
                }
                else
                {
                    PlayAgain = false;
                    vm.Messages.Add(GameResources.MESSAGE_USER_EXIT);
                    return vm;
                }
            }

            vm.Messages.Add(GameResources.MESSAGE_PLAY_AGAIN);
            return vm;
        }

        public BattleViewModel NewGame()
        {
            GameRepository.Ships.Clear();
            GameRepository.AttackedLocations.Clear();

            for (int s = 0; s < this.GameOptions.NumberOfBattleships; s++)
            {
                if(!GameRepository.AddShip(ShipType.BattleShip, GameOptions.BoardSize))
                {
                    throw new System.InvalidOperationException("Cannot place ship: " + ShipType.BattleShip);
                }
            }

            for (int s = 0; s < this.GameOptions.NumberOfDestroyers; s++)
            {
                if (!GameRepository.AddShip(ShipType.Destroyer, GameOptions.BoardSize))
                {
                    throw new System.InvalidOperationException("Cannot place ship: " + ShipType.Destroyer);
                }
            }

            string[] messages = new string[]
            {
                String.Format(GameResources.MESSAGE_WELCOME, GameRepository.Ships.Count),
                GameResources.MESSAGE_ATTACK
            };

            return new BattleViewModel(messages);
        }

        private bool CheckGameOver()
        {
            var emptyAttackLocations = GameOptions.BoardSize * GameOptions.BoardSize - (GameRepository.Ships.Sum(s => s.Size));
            if (emptyAttackLocations == GameRepository.AttackedLocations.Count)
                return true;

            if (GameRepository.Ships.Sum(s => s.Hits) == GameRepository.Ships.Sum(s => s.Size))
                return true;

            return false;
        }

        private BattleViewModel GetAttackResult(BattleViewModel vm)
        {
            Ship damagedShip = GameRepository.GetShipByLocation(vm.AttackLocation.xPos, vm.AttackLocation.yPos);

            if (damagedShip != null)
            {
                damagedShip.Hits++;
                if (damagedShip.Hits == damagedShip.Size)
                {
                    if (CheckGameOver())
                    {
                        vm.GameOver = true;
                        vm.Messages.Add(GameResources.MESSAGE_GAME_OVER_PLAYER_WIN);
                        vm.Messages.Add(GameResources.MESSAGE_PLAY_AGAIN);
                        return vm;
                    }

                    vm.Messages.Add(GameResources.MESSAGE_SUNK_SHIP);
                    vm.ShipsSunk++;
                    return vm;
                }
                else
                {
                    vm.Messages.Add(GameResources.MESSAGE_HIT_SHIP);
                    return vm;
                }
            }

            if (CheckGameOver())
            {
                vm.GameOver = true;
                vm.Messages.Add(GameResources.MESSAGE_GAME_OVER_COMPUTER_WIN);
                vm.Messages.Add(GameResources.MESSAGE_PLAY_AGAIN);
                return vm;
            }

            vm.Messages.Add(GameResources.MESSAGE_MISSED_SHIP);
            return vm;
        }
    }
}
