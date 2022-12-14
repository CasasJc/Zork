using Newtonsoft.Json;
using System;
using System.Linq;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }
        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            Output.WriteLine("Welcome to Zork!");
            Look();
            Output.WriteLine($"{Player.CurrentRoom}");
            Player.PlayerHealth = 10;
            Player.PlayerHunger = 0;
            Output.WriteLine($"Current health is {Player.PlayerHealth} & Current Hunger is {Player.PlayerHunger}");
        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string verb;
            string subject = null;
            if (commandTokens.Length == 0)
            {
                return;
            }
            else if (commandTokens.Length == 1)
            {
                verb = commandTokens[0];
            }
            else
            {
                verb = commandTokens[0];
                subject = commandTokens[1];
            }

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Output.WriteLine("Thank you for playing!");                   
                    break;

                case Commands.Look:
                    Look();
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    Player.PlayerHunger += 1;
                    HealthRestraints();
                    HungerRestraints();

                    break;

                case Commands.Take:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                    }
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                    }
                    break;

                case Commands.Eat:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Eat(subject);
                    }
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                        }
                    }
                    break;

                case Commands.Reward:
                    Player.PlayerScore++;
                    break;

                case Commands.Score:
                    Output.WriteLine($"Your score would be {Player.PlayerScore}, in {Player.MovesNumb} move(s)");
                    break;

                case Commands.Health:
                    Output.WriteLine($"Current health is {Player.PlayerHealth}");
                    break;

                case Commands.Hunger:
                    Output.WriteLine($"Current Hunger is {Player.PlayerHunger}");
                    break;

                case Commands.Stats:
                    Output.WriteLine($"Current health is {Player.PlayerHealth} & Current Hunger is {Player.PlayerHunger}");
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;

            }
            if (command != Commands.Unknown)
            {
                Player.MovesNumb++;
            }
            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }
        }
        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
        }

        private void Take(string itemName)
        {
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }
            else
            {
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Output.WriteLine("Taken.");
            }
        }
        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Output.WriteLine("Dropped.");
            }
        }
        private void Eat(string itemName)
        {
            Item itemToEat = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToEat == null)
            {
                Output.WriteLine("You can't see any such thing.");
            }

            if (itemToEat.Edible == false)
            {
                Output.WriteLine("This Object isnt edible");
            }

            if (itemToEat.Edible == true && itemToEat.BadEdible == true)
            {
                Player.RemoveItemFromInventory(itemToEat);
                Output.WriteLine($"{itemToEat.Name} Eaten.");
                Player.PlayerHunger += 2;
                Player.PlayerHealth -= 3;
                Output.WriteLine($"Current health is {Player.PlayerHealth} & Current Hunger is {Player.PlayerHunger}");
            }
           
            if (itemToEat.Edible == true && itemToEat.BadEdible == false)
            {
                Player.RemoveItemFromInventory(itemToEat);
                Output.WriteLine($"{itemToEat.Name} Eaten.");
                Player.PlayerHunger -= 2;
                Player.PlayerHealth += 2;
                Output.WriteLine($"Current health is {Player.PlayerHealth} & Current Hunger is {Player.PlayerHunger}");
            }
      
        }

        public void HealthRestraints()
        {
            if (Player.PlayerHealth < 1)
            {
                IsRunning = false;
                Output.WriteLine("You ran out of health!");
            }
            if (Player.PlayerHealth == 4)
            {
                Output.WriteLine($"You have {Player.PlayerHealth} Health, better find something to eat");
            }

            if (Player.PlayerHealth >= 10)
            {
                Player.PlayerHealth = 10;
            }
        }

        public void HungerRestraints()
        {
            if (Player.PlayerHunger >= 15)
            {
                IsRunning = false;
                Output.WriteLine("You Starved to death!");
            }
            if (Player.PlayerHunger == 10)
            {
                Output.WriteLine($"You have {Player.PlayerHunger} hunger, better find something to eat");
            }

            if(Player.PlayerHunger <= 1)
            {
                Player.PlayerHunger = 1;
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}