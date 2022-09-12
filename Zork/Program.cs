using System;
using System.Runtime.CompilerServices;

namespace Zork
{
    class Program
    {

        private static string CurrentRoom
        {
            get
            {
                return _rooms[Location.Row, Location.Column];
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);
                command = ToCommand(Console.ReadLine().Trim());
                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;
                    case Commands.LOOK:
                        outputString = "A rubber mat saying 'Welcome to Zork!' Lies by the door.";
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (Move(command))
                        {
                            outputString = $"You moved {command}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    default:
                        outputString = "Unkown command";
                        break;
                }
                Console.WriteLine(outputString);
            }
        }
        static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        }

        private static bool Move(Commands command)
        {
            bool didMove = false;
            switch (command)
            {
                case Commands.NORTH when Location.Row < _rooms.GetLength(0) - 1:
                    Location.Row++;
                    didMove = true;
                    break;

                case Commands.SOUTH when Location.Row > 0:
                    Location.Row--;
                    didMove = true;
                    break;

                case Commands.EAST when Location.Column < _rooms.GetLength(1) - 1:
                    Location.Column++;
                    didMove = true;
                    break;

                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    didMove = true;
                    break;
            }

            return didMove;
        }

        private static readonly string[,] _rooms =
        {
            {"Rocky Trail", "South of House", "Canyon View"},
            { "Forest", "West of House", "Behind House", },
            {"Dense Woods", "North of House", "Clearing"}
        };



        private static (int Row, int Column) Location = (1, 1);
    }
}
