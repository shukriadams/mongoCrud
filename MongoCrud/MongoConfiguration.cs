namespace MongoCrud
{
    /// <summary>
    /// Convenient global holder for all my own mongo config data. 
    /// </summary>
    public class MongoConfiguration
    {
        public string DatabaseName { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string DatabaseUsername { get; set; }

        public string DatabasePassword { get; set; }
    }
}
