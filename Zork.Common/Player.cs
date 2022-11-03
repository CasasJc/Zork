using System;
using System.Collections.Generic;

namespace Zork.Common
{
    public class Player
    {
        public Room CurrentRoom
        {
            get => _currentRoom;
            set => _currentRoom = value;
        }

       


        public List<Item> Inventory { get; }

        public Player(World world, string startingLocation)
        {
            _world = world;

            if (_world.RoomsByName.TryGetValue(startingLocation, out _currentRoom) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }

            Inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool didMove = _currentRoom.Neighbors.TryGetValue(direction, out Room neighbor);
            if (didMove)
            {
                CurrentRoom = neighbor;
            }

            return didMove;
        }



        public bool TakeItem(string NameOfItem)
        {
            Item TakeableItem = _world.ItemsByName.GetValueOrDefault(NameOfItem);
            if (CurrentRoom.Inventory.Contains(TakeableItem))
            {
                Inventory.Add(TakeableItem);
                CurrentRoom.Inventory.Remove(TakeableItem);
                return true;
            }
            else
            return false;         
        }

        public bool DropItem(string NameOfItem)
        {
            Item TakeableItem = _world.ItemsByName.GetValueOrDefault(NameOfItem);

            if (Inventory.Contains(TakeableItem))
            {
                Inventory.Remove(TakeableItem);
                CurrentRoom.Inventory.Add(TakeableItem);
                return true;
            }
            else           
            return false;        
        }

        private World _world;
        private Room _currentRoom;
    }
}
