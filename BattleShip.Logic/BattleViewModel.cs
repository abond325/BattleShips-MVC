using System.Collections.Generic;

namespace BattleshipLogic
{
    public class BattleViewModel 
    {
        public BattleViewModel()
        {
            Messages = new List<string>();
        }

        public BattleViewModel(string[] messages)
        {
            Messages = new List<string>();
            foreach (var msg in messages)
            {
                Messages.Add(msg);
            }
        }

        public bool GameOver { get; set; } = false;
        public bool InputIsValid { get; set; }
        public Location AttackLocation { get; set; }
        public List<Location> AttackedLocations { get; set; }
        public int ShipsSunk { get; set; }
        public List<string> Messages { get; set; }
    }
}
