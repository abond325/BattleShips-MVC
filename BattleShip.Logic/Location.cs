using System;
using System.Collections.Generic;

namespace BattleshipLogic
{
    public class Location : ValueObject<Location>
    {
        public int xPos { get; private set; }
        public int yPos { get; private set; }
        public bool HeadingNorth { get; set; }

        
        public Location(int x, int y, bool h = false)
        {
            xPos = x;
            yPos = y;
            HeadingNorth = h;
        }

        public override string ToString()
        {
            return LocationTranslatior.TranslateGameToInputLocation(this);
        }

        protected override IEnumerable<object> GetAttributesToInclideInEqualityCheck()
        {
            return new List<Object> { xPos, yPos };
        }
    }
}
