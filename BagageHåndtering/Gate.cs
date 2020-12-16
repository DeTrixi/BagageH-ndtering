using System;
using System.Collections.Generic;
using System.Threading;

namespace BagageHåndtering
{
    public class Gate
    {
        private readonly Queue<LuggageModel> _luggageModels;
        public int GateId { get; private set; }

        public Gate(int gateId, Queue<LuggageModel> luggageModels)
        {
            _luggageModels = luggageModels;
            GateId = gateId;
            new Thread((o => { MoveLuggageToThePlain(); })).Start();
        }

        
        public void MoveLuggageToThePlain()
        {
            do
            {
                lock (_luggageModels)
                {
                    while (_luggageModels.Count <= 0) Monitor.Wait(_luggageModels);
                    {
                        Console.WriteLine(
                            $"Moving luggage from gate: {GateId} to the plain {_luggageModels.Dequeue()}");
                    }
                    Monitor.PulseAll(_luggageModels);
                }

            } while (true);

        }
    }
}