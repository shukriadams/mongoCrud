using MongoDB.Bson;

namespace MongoCrud
{
    public abstract class MongoEntity
    {
        public abstract BsonDocument Serialize();

        public abstract void Deserialize(BsonDocument document);
    }
}
