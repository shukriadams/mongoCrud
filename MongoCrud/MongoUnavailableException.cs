using System;

namespace MongoCrud
{
    /// <summary>
    /// Exception thrown when mongodb cannot be connected to. Use for graceful handling of backend disconnects.
    /// </summary>
    public class MongoUnavailableException : Exception
    {
        public MongoUnavailableException()
        {

        }

        public MongoUnavailableException(Exception ex) : base("Mongo exception", ex)
        {
            
        }
    }
}
