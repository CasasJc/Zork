using System;
using System.Collections.Generic;
using System.IO;

namespace Zork
{
    class Program
    {
        private static Room CurrentRoom
        {
            get
            {
                return _rooms[Location.Row, Location.Column];
            }
        }
        static void Main(string[] args)
        {
            const string defaultRoomsFilename = "Content\\Rooms.txt";
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFilename] : defaultRoomsFilename);
            InitializeRoomDescriptions(roomsFilename);
            Console.WriteLine("Welcome to Zork!");
            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);
                if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
                command = ToCommand(Console.ReadLine().Trim());
                string outputString;
                switch (command)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;
                    case Commands.LOOK:
                        outputString = CurrentRoom.Description;
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
                        outputString = "Unkown";
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
        private static (int Row, int Column) Location = (1, 1);
        private static readonly Room[,] _rooms =
        {
            {new Room("Rocky Trail"),new Room("South of House"), new Room("Canyon View")},
            {new Room("Forest"),new Room("West of House"), new Room("Behind House")},
            {new Room("Dense Woods"), new Room("North of House"), new Room("Clearing")}
        };
        private static void InitializeRoomDescriptions(string roomsFilename)
        {
          const string fieldDelimiter = "##";
          const int expectedFieldCount = 2;
          string[] lines = File.ReadAllLines(roomsFilename);
          foreach (string line in lines)
          {
              string[] fields = line.Split(fieldDelimiter);
              if (fields.Length != expectedFieldCount)
              {
                  throw new InvalidCastException("Invalid record.");
              }
              string name = fields[(int)Fields.Name];
              string description = fields[(int)Fields.Description];
              roomMap[name].Description = description;
          }
        }
        private static readonly Dictionary<string, Room> roomMap;
        static Program()
        {
            roomMap = new Dictionary<string, Room>();
            foreach (Room room in _rooms)
            {
                roomMap[room.Name] = room;
            }
        }
        private enum Fields
        {
            Name = 0,
            Description
        }
        private enum CommandLineArguments
        {
            RoomsFilename = 0
        }
    }
}
