using BattleshipLogic.Interfaces;
using System;
using System.Collections.Generic;

namespace BattleshipLogic
{
    public class GameRepository : IGameRepository
    {
        public List<Ship> Ships { get; private set; }
        public List<Location> AttackedLocations { get; private set; }

        public GameRepository()
        {
            Ships = new List<Ship>();
            AttackedLocations = new List<Location>();
        }
        public Ship GetShipByLocation(int x, int y)
        {
            foreach (var ship in Ships)
            {
                if (ship.IsAtLocation(x, y))
                    return ship;
            }

            return null;
        }
        public bool AddShip(ShipType type, int boardSize)
        {
            Ship s = null;
            switch (type)
            {
                case ShipType.BattleShip:
                    s = new Battleship();
                    break;
                case ShipType.Destroyer:
                    s = new Destroyer();
                    break;
            }

            Random rnd = new Random();
            var maxCoordinate = boardSize - 1;

            var x = 0; var y = 0; var headingNorth = true;
            var canPlaceShip = false; var attemptedToPlace = 0;
            while (!canPlaceShip && attemptedToPlace < 20)
            {
                canPlaceShip = true;
                attemptedToPlace++;
                x = rnd.Next(0, maxCoordinate);
                y = rnd.Next(0, maxCoordinate);
                headingNorth = (rnd.Next(0, 2) == 1) ? true : false;

                if (headingNorth)
                {
                    if (y <= (maxCoordinate - s.Size))
                    {
                        for (int i = 0; i < s.Size; i++)
                        {
                            if (GetShipByLocation(x, y + i) != null)
                                canPlaceShip = false;
                        }
                    }
                    else
                    {
                        canPlaceShip = false;
                    }
                }
                else
                {
                    if (x <= (maxCoordinate - s.Size))
                    {
                        for (int i = 0; i < s.Size; i++)
                        {
                            if (GetShipByLocation(x + i, y) != null)
                                canPlaceShip = false;
                        }
                    }
                    else
                    {
                        canPlaceShip = false;
                    }
                }

            }
            if (canPlaceShip)
            {
                s.Coordinates = new Location(x, y, headingNorth);
                Ships.Add(s);
            }
            return canPlaceShip;
        }
    }

}
