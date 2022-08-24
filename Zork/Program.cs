﻿using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            string inputString = Console.ReadLine().Trim();
            inputString = inputString.ToUpper();
            if(inputString == "QUIT")
            {
                Console.WriteLine("Thank you for playing.");
            }
            else if (inputString == "LOOK")
            {
                Console.WriteLine("This is an open field west of a white house, whith a boarded front door.\nA rubber mat saying 'Welcome to Zork!' Lies by the door.");
            }
            else
            {
                Console.WriteLine("Unrecognized command.");

            }

        }
    }
}
