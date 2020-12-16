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


            for (int i = 0; i < 3; i++)
            {
                // Creates a service desk for each loop
                checkins.Add(new(i + 1));
                // Gates to transport luggage to the airport
                // TODO VERIFY THAT IT IS A POINTER TO LIST
                Queue<LuggageModel> luggage = new Queue<LuggageModel>();
                gates.Add(new Gate(i + 1, luggage));
                LuggageAtTheGates.Add(luggage);
            }

            // This is a distributor that moves fro checkin to checkout
            Distributor distributor = new Distributor(LuggageFromCheckIn, LuggageAtTheGates);

            do
            {
                Console.WriteLine("GAME ON!");
                Console.WriteLine($"Current Thread Count Running: {Process.GetCurrentProcess().Threads.Count}");
                Thread.Sleep(10000);
            } while (true);
        }
    }

    public class Distributor
    {
        private readonly Queue<LuggageModel> _luggageFromCheckIn;
        private readonly List<Queue<LuggageModel>> _luggageAtTheGates;

        /// <summary>
        /// Constructor takes in the luggage lists so that it can distribute the luggage
        /// It also starts a thread to continue running the method in its own string
        /// </summary>
        /// <param name="luggageFromCheckIn"></param>
        /// <param name="luggageAtTheGates"></param>
        public Distributor(Queue<LuggageModel> luggageFromCheckIn, List<Queue<LuggageModel>> luggageAtTheGates)
        {
            _luggageFromCheckIn = luggageFromCheckIn;
            _luggageAtTheGates = luggageAtTheGates;
            new Thread(StartDistributor).Start();
        }

        private void StartDistributor()
        {
            do
            {
                lock (_luggageFromCheckIn)
                {
                    // Checks for the count in the checkin queue and waits if it is empty
                    while (_luggageFromCheckIn.Count <= 0) Monitor.Wait(_luggageFromCheckIn);
                    {
                        foreach (var luggageModel in _luggageFromCheckIn)
                        {
                            // TODO Check for id
                            // TODO Lock the list with that id
                            // TODO Move from one queue to another
                            // TODO Unlock Again
                            Console.WriteLine($"{luggageModel.Id} , {luggageModel.Type}");
                            Thread.Sleep(500);
                        }
                    }
                    Monitor.PulseAll(_luggageFromCheckIn);
                }
            } while (true);
        }
    }
}