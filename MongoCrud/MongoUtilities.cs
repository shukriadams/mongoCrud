using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Collections.Generic;

namespace MongoCrud
{
    public class MongoUtilities
    {
        #region FIELDS

        private MongoDb _mongo;

        #endregion

        #region CTORS

        public MongoUtilities(MongoDb mongodb)
        {
            _mongo = mongodb;
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Initializes database. This is not fast, so avoid calling it constantly. Ideally, this method is called
        /// when your app starts up.
        /// </summary>
        public void InitializeDatabase()
        {
            MongoDefine instance = MongoTypeProvider.GetDefinition();
            IEnumerable<MCollection> collections = instance.Define();

            foreach (MCollection collection in collections)
            {
                if (!_mongo.CollectionExists(collection.Name))
                    _mongo.CreateCollection(collection.Name);

                // set up indexes
                if (collection.Indices.Length > 0)
                    _mongo.GetCollection<BsonDocument>(collection.Name).CreateIndex(IndexKeys.Ascending(collection.Indices), IndexOptions.SetUnique(true));

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void TruncateAllCollections()
        {
            MongoDefine instance = MongoTypeProvider.GetDefinition();
            IEnumerable<MCollection> collections = instance.Define();

            foreach (MCollection collection in collections)
            {
                if (_mongo.CollectionExists(collection.Name))
                    _mongo.GetCollection(collection.Name).RemoveAll();
            }
        }

        #endregion

    }
}
