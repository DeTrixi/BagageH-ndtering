using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;

namespace BagageHåndtering
{
    partial class Program
    {
        // Luggage from checkin
        static readonly Queue<LuggageModel> LuggageFromCheckIn = new();

        // List of luggage ques at the gates
        static readonly List<Queue<LuggageModel>> LuggageAtTheGates = new();


        static void Main(string[] args)
        {
            Console.WriteLine("Hello Teacher!");
            // This i a list of Checkins for the luggage
            List<CheckIn> checkins = new();
            // This i a list of gates to handle luggage leaving the airport
            List<Gate> gates = new();


            // This loop Creates the wished amount of gates and Check in
            for (int i = 0; i < 3; i++)
            {
                // Creates a service desk for each loop
                checkins.Add(new(i + 1));
                // Gates to transport luggage to the airport
                Queue<LuggageModel> luggage = new Queue<LuggageModel>();
                // This line adds a new gate to the system
                gates.Add(new Gate(i + 1, luggage));
                // this line adds a new Queue of luggage model to the list of LuggageAtTheGates
                LuggageAtTheGates.Add(luggage);
            }

            // This is a distributor that moves fro checkin to checkout
            Distributor distributor = new Distributor(LuggageFromCheckIn, LuggageAtTheGates);

            do
            {
                // This loop if for my own debugging purpose
                Console.WriteLine("GAME ON!");
                Console.WriteLine($"Current Thread Count Running: {Process.GetCurrentProcess().Threads.Count}");
                Thread.Sleep(10000);
            } while (true);
        }
    }
}