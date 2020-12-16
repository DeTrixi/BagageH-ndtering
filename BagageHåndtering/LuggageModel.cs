namespace BagageHåndtering
{
    /// <summary>
    /// THis class is a piece of luggage
    /// </summary>
    public class LuggageModel
    {
        // Luggage Id or route to gate
        public int Id { get; private set; }

        // Luggage type lie trunk or backpack
        public string Type { get; private set; }

        public LuggageModel(int id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}