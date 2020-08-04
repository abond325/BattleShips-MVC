using BattleshipLogic;
using System;
using System.Text;

namespace BattleshipClient
{
    class Program
    {
        private static BattleViewModel Model;
        static void Main(string[] args)
        {
            //Setup
            BattleshipsGameBoard gb = new BattleshipsGameBoard(
                new GameResources(),
                new GameOptions(),
                new GameRepository()
                );            
            
            Console.Title = gb.Title;
            Model = gb.NewGame();
            RenderViewModel(Model);

            //Game loop
            while (!Model.GameOver)
            {
                BattleViewModel result = gb.LaunchAttack(Console.ReadLine());
                RenderViewModel(result);
                Console.Title = gb.Title + $" - Attacked locations: {result.AttackedLocations?.Count}, {result.ShipsSunk} ship(s) sunk";
            }
        }
        private static void RenderViewModel(BattleViewModel model)
        {
            if (model.AttackedLocations != null)
            {
                StringBuilder sb = new StringBuilder("Attacked locations: ");
                foreach (var loc in model.AttackedLocations)
                {
                    sb.Append(" " + loc.ToString());
                }
                Console.WriteLine(sb.ToString());
            }

            foreach (var msg in model.Messages)
            {
                Console.WriteLine(msg);
            }            
        }
    }
}
