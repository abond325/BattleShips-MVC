using Microsoft.VisualStudio.TestTools.UnitTesting;
using BattleshipLogic;
using System.Collections.Generic;
using System.Linq;
using BattleshipLogic.Interfaces;
using System;

namespace Battleship.Tests
{
    [TestClass]
    public class GameBoardTests
    {
        public static BattleshipsGameBoard game;        

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            game = new BattleshipsGameBoard(
                new MockGameResources(),
                new MockGameOptions(),
                new MockGameRepository()
                );
        }

        [TestMethod]
        public void Game_instance_has_EntityId()
        {
            Assert.AreEqual("BattleshipsGameBoard0", game.Id);
        }

        [TestMethod]
        public void Game_instance_has_GameOptions_and_GameResource()
        {
            Assert.IsNotNull(game.GameOptions);
            Assert.IsNotNull(game.GameResources);
        }

        [TestMethod]
        public void Can_access_GameResources()
        {
            var boardSize = 10;

            var boardSizeResult = game.GameOptions.BoardSize;

            Assert.AreEqual(boardSize, boardSizeResult);
        }

        [TestMethod]
        public void Two_locations_with_same_coordinates_are_equal()
        {
            Location locA = new Location(10,10);
            Location locB = new Location(10,10);

            var result = locA.Equals(locB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Two_different_types_of_ship_with_same_coordinates_are_equal()
        {
            Ship shipA = new Destroyer { Coordinates = new Location(10, 10) };
            Ship shipB = new BattleshipLogic.Battleship { Coordinates = new Location(10, 10) };

            var result = shipA.Equals(shipB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Two_ships_with_same_coordinates_are_equal()
        {
            Ship shipA = new Destroyer { Coordinates = new Location(10, 10) };
            Ship shipB = new Destroyer { Coordinates = new Location(10, 10) };

            var result = shipA.Equals(shipB);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Can_access_GameOptions()
        {
            var expected_attack_msg = $"Enter attack coordinates [A-J][0-9]: ";
            var expected_attacked_msg = $"You already attacked this position, attack something else.";

            var attack_msg = game.GameResources.MESSAGE_ATTACK;
            var attacked_msg = game.GameResources.MESSAGE_ALREADY_ATTACKED;

            Assert.AreEqual(expected_attack_msg, attack_msg);
            Assert.AreEqual(expected_attacked_msg, attacked_msg);
        }

        [TestMethod]
        public void Test_game_over_when_all_ship_are_sunk()
        {
            game.NewGame();
            var expectedResult = game.GameResources.MESSAGE_GAME_OVER_PLAYER_WIN;
            BattleViewModel result = null;

            foreach (var ship in game.GameRepository.Ships)
            {
                for (int i = 0; i < ship.Size; i++)
                {
                    if (ship.Coordinates.HeadingNorth)
                    {
                        result = game.LaunchAttack(char.ConvertFromUtf32(ship.Coordinates.xPos + 65) + (ship.Coordinates.yPos + i));
                    }
                    else
                    {
                        result = game.LaunchAttack(char.ConvertFromUtf32(ship.Coordinates.xPos + 65 + i) + (ship.Coordinates.yPos));
                    }
                    
                }              
            }

            //Assert.AreEqual(expectedResult, result);
            Assert.AreEqual(game.GameRepository.Ships.Sum(s => s.Size), game.GameRepository.Ships.Sum(s => s.Hits));
        }

        [TestMethod]
        public void Test_game_over_when_all_empty_squares_have_been_attacked()
        {
            game.NewGame();
            var expectedResult = game.GameResources.MESSAGE_GAME_OVER_COMPUTER_WIN;
            string validXPos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            BattleViewModel result = null;

            for (int x = 0; x < game.GameOptions.BoardSize; x++)
            {
                for (int y = 0; y < game.GameOptions.BoardSize; y++)
                {
                    Ship s = null;
                    foreach (var ship in game.GameRepository.Ships)
                    {
                        for (int i = 0; i < ship.Size; i++)
                        {
                            if (ship.Coordinates != null)
                            {
                                if (!ship.Coordinates.HeadingNorth)
                                {
                                    if (ship.Coordinates.xPos + i == x && ship.Coordinates.yPos == y)
                                        s = ship;
                                }
                                else
                                {
                                    if (ship.Coordinates.xPos == x && ship.Coordinates.yPos + i == y)
                                        s = ship;
                                }
                            }
                        }
                    }

                    if(s == null)
                        result = game.LaunchAttack(validXPos.Substring(x, 1) + y);
                }

            }

            Assert.AreEqual(expectedResult, result.Messages[0]);
        }
    }

    public class MockGameRepository : IGameRepository
    {
        public List<Ship> Ships { get; private set; }
        public List<Location> AttackedLocations { get; private set; }


        public MockGameRepository()
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
                    s = new BattleshipLogic.Battleship();
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
                headingNorth = (rnd.Next(0, 2) == 1);

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

    public class MockGameOptions : IGameOptions
    {
        public int BoardSize { get; } = 10;
        public int NumberOfBattleships { get; } = 1;
        public int NumberOfDestroyers { get; } = 2;
    }

    public class MockGameResources : IGameResources
    {
        public string MESSAGE_ATTACK { get; } = $"Enter attack coordinates [A-J][0-9]: ";
        public string MESSAGE_HIT_SHIP { get; } = $"You hit a ship.";
        public string MESSAGE_SUNK_SHIP { get; } = $"You sunk the computer's ship.";
        public string MESSAGE_MISSED_SHIP { get; } = $"MISS - Enter attack coordinates [A-J][0-9]: ";
        public string MESSAGE_ALREADY_ATTACKED { get; } = $"You already attacked this position, attack something else.";
        public string MESSAGE_INPUT_ERROR_LENGTH { get; } = $"INPUT ERROR: Enter attack coordinates [A-J][0-9], example A0:  ";
        public string MESSAGE_INPUT_ERROR_CHAR { get; } = $"INPUT ERROR: Enter attack coordinates [A-J][0-9], example A0:";
        public string MESSAGE_INPUT_ERROR_OUT_OF_RANGE { get; } = "INPUT OUTSIDE GAME BOARD: Board size is {0}";
        public string MESSAGE_GAME_OVER_COMPUTER_WIN { get; } = "GAME OVER - COMPUTER WINS";
        public string MESSAGE_GAME_OVER_PLAYER_WIN { get; } = "GAME OVE - PLAYER WINS";
        public string APPLICATION_TITLE { get; } = "Battleships";
        public string MESSAGE_WELCOME { get; } = "New game, the computer has placed {0} ships.";
        public string MESSAGE_PLAY_AGAIN { get; } = "Play again (Y/N)";
        public string MESSAGE_USER_EXIT { get; } = "GAME OVE - PLAYER QUITS";
    }
}
