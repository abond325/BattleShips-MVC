using System.Collections.Generic;

namespace BattleshipLogic.Interfaces
{
    public interface IGameRepository
    {
        List<Location> AttackedLocations { get; }
        List<Ship> Ships { get; }
        Ship GetShipByLocation(int x, int y);
        bool AddShip(ShipType type, int boardSize);
    }
}