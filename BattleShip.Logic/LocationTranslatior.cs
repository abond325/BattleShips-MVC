using System;

namespace BattleshipLogic
{
    public static class LocationTranslatior
    {
        public static Location TranslateInputToGameLocation(string pos)
        {
            int x = pos.ToUpper()[0] - 65;
            int y = (int)Char.GetNumericValue(pos[1]);

            return new Location(x, y);
        }

        public static string TranslateGameToInputLocation(Location loc)
        {
            return (char)(loc.xPos + 65) + loc.yPos.ToString();
        }
    }
}
