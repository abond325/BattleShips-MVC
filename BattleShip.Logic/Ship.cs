using System;
using System.Collections.Generic;

namespace BattleshipLogic
{
    public class Ship : ValueObject<Ship>
    {
        public virtual int Size { get; }
        public int Hits { get; set; }
        public Location Coordinates { get; set; }

        public bool IsAtLocation(int x, int y)
        {
            for (int i = 0; i < Size; i++)
            {
                if (Coordinates != null)
                {
                    if (!Coordinates.HeadingNorth)
                    {
                        if (Coordinates.xPos + i == x && Coordinates.yPos == y)
                            return true;
                    }
                    else
                    {
                        if (Coordinates.xPos == x && Coordinates.yPos + i == y)
                            return true;
                    }
                }
            }
            return false;
        }

        protected override IEnumerable<object> GetAttributesToInclideInEqualityCheck()
        {
            return new List<Object> { this.Coordinates.xPos, this.Coordinates.yPos };
        }
    }
}
