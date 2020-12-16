using System;
using System.Threading;

namespace BagageHåndtering
{
    partial class Program
    {
        private class CheckIn
        {
            private int _checkinNumber;
            private Random ran = new();

            /// <summary>
            /// Constructor creates a self maintaining thread that produces luggage
            /// And imitates a constant flow of people
            /// </summary>
            /// <param name="checkinNumber"></param>
            public CheckIn(int checkinNumber)
            {
                _checkinNumber = checkinNumber;
                new Thread(CheckInLuggage).Start();
            }

            /// <summary>
            /// Method that imitate a constant flow of people
            /// </summary>
            private void CheckInLuggage()
            {
                do
                {
                    Thread.Sleep(10);
                    lock (LuggageFromCheckIn)
                    {
                        while (LuggageFromCheckIn.Count > 10)
                        {
                            Monitor.Wait(LuggageFromCheckIn);

                        }
                        // making sure that other threads also gets a chance to deliver luggage
                        for (int i = 0; i < 4; i++)
                        {
                            string type = ran.Next(2) == 0 ? "Trunk" : "BackPack";
                            LuggageFromCheckIn.Enqueue(new LuggageModel(_checkinNumber, type));
                        }

                        Monitor.PulseAll(LuggageFromCheckIn);

                    }
                } while (true);
            }
        }
    }
}