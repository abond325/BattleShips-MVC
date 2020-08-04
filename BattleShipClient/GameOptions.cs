using BattleshipLogic;
using BattleshipLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipClient
{
    public class GameOptions : IGameOptions
    {
        public int BoardSize { get; } = 10;
        public int NumberOfBattleships { get; } = 1;
        public int NumberOfDestroyers { get; } = 2;

    }
}
