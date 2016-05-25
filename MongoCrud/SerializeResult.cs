using MongoDB.Bson;

namespace MongoCrud
{
    public class SerializeResult
    {
        /// <summary>
        /// 
        /// </summary>
        public BsonDocument Document { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
    }
}
