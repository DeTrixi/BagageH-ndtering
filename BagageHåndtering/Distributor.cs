using System.Collections.Generic;
using System.Threading;

namespace BagageHåndtering
{
    public class Distributor
    {
        private readonly Queue<LuggageModel> _luggageFromCheckIn;
        private readonly List<Queue<LuggageModel>> _luggageAtTheGates;
        private readonly List<Queue<LuggageModel>> _luggageAtTheGatesTemp = new();


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
            InitTempList();
        }

        /// <summary>
        /// Crerate a temp list to all the luggage so i dont have to make a interlock
        /// </summary>
        private void InitTempList()
        {
            for (int i = 0; i < _luggageAtTheGates.Count; i++)
            {
                _luggageAtTheGatesTemp.Add(new Queue<LuggageModel>());
            }
        }

        /// <summary>
        /// The distributor sorts the luggage and ships it on to the gates
        /// </summary>
        private void StartDistributor()
        {
            do
            {
                lock (_luggageFromCheckIn)
                {
                    // Checks for the count in the checkin queue and waits if it is empty
                    while (_luggageFromCheckIn.Count <= 0) Monitor.Wait(_luggageFromCheckIn);
                    {
                        //Console.WriteLine($"{_luggageFromCheckIn.Peek().Id - 1} , {_luggageFromCheckIn.Peek().Type}");
                        // Sorts the list ino tree or more lists looking at the id in the luggage and adding them to the array below
                        _luggageAtTheGatesTemp[_luggageFromCheckIn.Peek().Id - 1]
                            .Enqueue(_luggageFromCheckIn.Dequeue());

                        Thread.Sleep(500);
                    }
                    Monitor.PulseAll(_luggageFromCheckIn);
                }

                // Starts a new thread for each Terminal
                for (int i = 0; i < _luggageAtTheGatesTemp.Count; i++)
                {
                    int t = i;
                    if (_luggageAtTheGatesTemp[i].Count > 0)
                    {
                        new Thread(o => MoveToTHeTerminal(_luggageAtTheGatesTemp[t], t)).Start();
                    }
                }
            } while (true);
        }

        /// <summary>
        /// Moves all the luggage to the right location
        /// </summary>
        /// <param name="luggage"></param>
        /// <param name="listToLock"></param>
        private void MoveToTHeTerminal(Queue<LuggageModel> luggage, int listToLock)
        {
            lock (_luggageAtTheGates[listToLock])
            {
                while (luggage.Count <= 0) Monitor.Wait(_luggageAtTheGates[listToLock]);
                {
                    if (luggage.Count > 0)
                    {
                        _luggageAtTheGates[listToLock].Enqueue(luggage.Dequeue());
                    }
                }

                Monitor.PulseAll(_luggageAtTheGates[listToLock]);
            }
        }
    }
}