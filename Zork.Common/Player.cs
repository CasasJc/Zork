using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Player
    {
        public event EventHandler<Room> LocationChanged;
        public event EventHandler<int> MovesChanged;
        public event EventHandler<int> ScoreChanged;
        public event EventHandler<int> HealthChange;
        public event EventHandler<int> HungerChange;
        private int _movesNumber;
        private int _Score;
        private int _Health;
        private int _Hunger;

        public Room CurrentRoom
        {         
            get => _currentRoom;
            set
            {
                if(_currentRoom != value)
                {
                    _currentRoom = value;
                    LocationChanged?.Invoke(this, _currentRoom);
                }
            }
        }
        public int MovesNumb
        {
            get => _movesNumber;
            set
            {
                if(_movesNumber != value)
                {
                    _movesNumber = value;
                    MovesChanged?.Invoke(this, _movesNumber);
                }
            }
        }
        public int PlayerScore
        {
            get => _Score;
            set
            {
                if (_Score != value)
                {
                    _Score = value;
                    ScoreChanged?.Invoke(this, _Score);
                }
            }
        }

        public int PlayerHealth
        {
            get => _Health;
            set
            {
                if (_Health != value)
                {
                    _Health = value;
                    HealthChange?.Invoke(this, _Health);
                }
            }
        }
        public int PlayerHunger
        {
            get => _Hunger;
            set
            {
                if (_Hunger != value)
                {
                    _Hunger = value;
                    HungerChange?.Invoke(this, _Hunger);
                }
            }
        }

        public IEnumerable<Item> Inventory => _inventory;
        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            _inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
                //MoveGain();
            }

            return didMove;
        }

        public void AddItemToInventory(Item itemToAdd)
        {
            if (_inventory.Contains(itemToAdd))
            {
                throw new Exception($"Item {itemToAdd} already exists in inventory.");
            }

            _inventory.Add(itemToAdd);
        }

        public void RemoveItemFromInventory(Item itemToRemove)
        {
            if (_inventory.Remove(itemToRemove) == false)
            {
                throw new Exception("Could not remove item from inventory.");
            }
        }

        private readonly World _world;
        private Room _currentRoom;
        private readonly List<Item> _inventory;
    }
}
