using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection.Emit;

namespace MongoCrud
{
    /// <summary>
    /// Allows neat wrapping of calls so database calls can be 
    /// counted, and unavailable databases can be cleanly handled.
    /// 
    /// This wrapper is entirely optional - use Mongodatabase directly if wanted.
    /// </summary>
    public class MongoDb
    {
        #region FIELDS

        private MongoDatabase Database { get; set; }

        private ICache _cache = new PlaceholderCache();

        #endregion

        #region CTORS

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MongoDb(MongoConfiguration config)
        {
            try
            {
                MongoCredential credential = MongoCredential.CreateMongoCRCredential(
                    config.DatabaseName,
                    config.DatabaseUsername,
                    config.DatabasePassword);

                MongoClientSettings clientSettings = new MongoClientSettings
                {
                    Credentials = new[] { credential }
                };
                MongoServerSettings settings = new MongoServerSettings();

                MongoServer server = new MongoServer(settings);
                server.GetServerInstance(new MongoServerAddress(config.Host, config.Port));


                this.Database = server.GetDatabase(config.DatabaseName);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public MongoDb(string connectionString, string databaseName)
        {
            try
            {
                MongoClient client = new MongoClient(connectionString);
                MongoServer server = client.GetServer();
                this.Database = server.GetDatabase(databaseName);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cache"></param>
        public void SetCache(ICache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetCacheKey(string id)
        {
            return string.Format("objId:{0}", id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetTypeProperties(Type type)
        {
            string key = "typeProperties"+ type.FullName;
            IEnumerable<PropertyInfo> properties = Cache.Instance.Get(key) as IEnumerable<PropertyInfo>;
            if (properties == null)
            {
                properties = type.GetProperties().Where(
                                   prop => Attribute.IsDefined(prop, typeof(DataPropertyAttribute)));
                Cache.Instance.Add(key, properties);
            }
            return properties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private DataPropertyAttribute GetCustomAttribute(Type type, PropertyInfo property)
        {
            string key = string.Format("attr:{0}:{1}:{2}", type.FullName, property.Name, typeof(DataPropertyAttribute).Name);

            DataPropertyAttribute attribute = Cache.Instance.Get(key) as DataPropertyAttribute;
            if (attribute == null)
            {
                attribute = property.GetCustomAttribute<DataPropertyAttribute>();
                Cache.Instance.Add(key, attribute);
            }
            return attribute;
                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="doc"></param>
        private void Deserialize(object entity, BsonDocument doc)
        {
            Type type = entity.GetType();

            IEnumerable<PropertyInfo> properties = this.GetTypeProperties(type);

            foreach (PropertyInfo property in properties)
            {
                DataPropertyAttribute attribute = GetCustomAttribute(type, property);

                if (attribute.IsIndex)
                {
                    property.SetValue(entity, doc["_id"].AsString);
                }
                else
                {
                    if (property.PropertyType == typeof(string))
                        property.SetValue(entity, CastTo.String(doc, property.Name));
                    else if (property.PropertyType == typeof(Boolean))
                        property.SetValue(entity, CastTo.Boolean(doc, property.Name));
                    else if (property.PropertyType == typeof(Boolean?))
                        property.SetValue(entity, CastTo.NullableBoolean(doc, property.Name));
                    else if (property.PropertyType == typeof(byte[]))
                        property.SetValue(entity, CastTo.ByteArray(doc, property.Name));
                    else if (property.PropertyType == typeof(Regex))
                        property.SetValue(entity, CastTo.Regex(doc, property.Name));
                    else if (property.PropertyType == typeof(int))
                        property.SetValue(entity, CastTo.Int(doc, property.Name));
                    else if (property.PropertyType == typeof(int?))
                        property.SetValue(entity, CastTo.NullableInt(doc, property.Name));
                    else if (property.PropertyType == typeof(long))
                        property.SetValue(entity, CastTo.Long(doc, property.Name));
                    else if (property.PropertyType == typeof(long?))
                        property.SetValue(entity, CastTo.NullableLong(doc, property.Name));
                    else if (property.PropertyType == typeof(double))
                        property.SetValue(entity, CastTo.Double(doc, property.Name));
                    else if (property.PropertyType == typeof(double?))
                        property.SetValue(entity, CastTo.NullableDouble(doc, property.Name));
                    else if (property.PropertyType == typeof(DateTime))
                        property.SetValue(entity, CastTo.DateTime(doc, property.Name));
                    else if (property.PropertyType == typeof(DateTime?))
                        property.SetValue(entity, CastTo.NullableDateTime(doc, property.Name));
                    else
                        throw new Exception(string.Format("CastTo error - property {0} of type {1} is not a supported data type", property.PropertyType, type.Name));
                }
            }
        }

        /// <summary>
        /// Serializes an entity to a BSON document
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="processIndex">True if index should be included. Must be false when updating.</param>
        /// <returns></returns>
        private SerializeResult Serialize(object entity, bool processIndex)
        {
            Type type = entity.GetType();

            IEnumerable<PropertyInfo> properties = this.GetTypeProperties(type);

            BsonDocument doc = new BsonDocument();
            string id = null;
            bool indexFound = false;

            foreach (PropertyInfo property in properties)
            {
                DataPropertyAttribute attribute = GetCustomAttribute(type, property);

                if (attribute.IsIndex)
                {
                    if (indexFound)
                        throw new Exception(string.Format("{0} has multiple properties marked as index. Only one index is allowed.", type.Name));

                    if (property.PropertyType != typeof(string))
                        throw new Exception(string.Format("Only a string property can be marked as index.", type.Name));

                    id = property.GetValue(entity) as string;
                    if (processIndex)
                        doc["_id"] = id;

                    indexFound = true;
                }
                else
                {
                    if (property.PropertyType == typeof(string))
                        doc[property.Name] = CastFrom.String(property.GetValue(entity));
                    else if (property.PropertyType == typeof(DateTime))
                        doc[property.Name] = CastFrom.DateTime(property.GetValue(entity));
                    else if (property.PropertyType == typeof(DateTime?))
                        doc[property.Name] = CastFrom.NullableDateTime(property.GetValue(entity));
                    else if (property.PropertyType == typeof(bool))
                        doc[property.Name] = CastFrom.Boolean(property.GetValue(entity));
                    else if (property.PropertyType == typeof(bool?))
                        doc[property.Name] = CastFrom.NullableBoolean(property.GetValue(entity));
                    else if (property.PropertyType == typeof(int))
                        doc[property.Name] = CastFrom.Int(property.GetValue(entity));
                    else if (property.PropertyType == typeof(int?))
                        doc[property.Name] = CastFrom.NullableInt(property.GetValue(entity));
                    else if (property.PropertyType == typeof(long))
                        doc[property.Name] = CastFrom.Long(property.GetValue(entity));
                    else if (property.PropertyType == typeof(long?))
                        doc[property.Name] = CastFrom.NullableLong(property.GetValue(entity));
                    else if (property.PropertyType == typeof(double))
                        doc[property.Name] = CastFrom.Double(property.GetValue(entity));
                    else if (property.PropertyType == typeof(double?))
                        doc[property.Name] = CastFrom.NullableDouble(property.GetValue(entity));
                    else if (property.PropertyType == typeof(Regex))
                        doc[property.Name] = CastFrom.Regex(property.GetValue(entity));
                    else if (property.PropertyType == typeof(byte[]))
                        doc[property.Name] = CastFrom.ByteArray(property.GetValue(entity));
                    else
                        throw new Exception(string.Format("CastFrom error - property {0} of type {1} is not a supported data type", property.PropertyType, type.Name));

                }
            }

            // todo : check id for null

            return new SerializeResult { Document = doc, Id = id };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(object entity)
        {
            try
            {
                Type type = entity.GetType();
                string name = type.Name;

                MongoCollection<BsonDocument> records = this.Database.GetCollection<BsonDocument>(name);
                SerializeResult ser = Serialize(entity, true);
                BsonDocument doc = ser.Document;
                records.Insert(doc);

                _cache.Add(GetCacheKey(ser.Id), entity);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Update(object entity)
        {
            try
            {
                Type type = entity.GetType();
                string name = type.Name;
                MongoCollection<BsonDocument> records = this.Database.GetCollection<BsonDocument>(name);
                SerializeResult ser = Serialize(entity, false);

                records.Update(Query.EQ("_id", ser.Id), new UpdateDocument { { "$set", ser.Document } });

                _cache.Add(GetCacheKey(ser.Id), entity);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="table"></param>
        public void Delete<T>(string id) 
        {
            try
            {
                MongoCollection<BsonDocument> records = this.Database.GetCollection<BsonDocument>(typeof(T).Name);
                records.Remove(Query.EQ("_id", id));

                _cache.Remove(GetCacheKey(id));
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <param name="table"></param>
        public void Delete(IMongoQuery q, string table) 
        {
            try
            {
                MongoCollection<BsonDocument> records = this.Database.GetCollection<BsonDocument>(table);
                records.Remove(q);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public T GetById<T>(string id)
        {
            try
            {
                string key = GetCacheKey(id);
                T entity = (T)_cache.Get(key);
                if (entity == null)
                {
                    MongoCollection<BsonDocument> records = this.Database.GetCollection<BsonDocument>(typeof(T).Name);
                    BsonDocument document = records.FindOneById(BsonValue.Create(id));

                    if (document == null)
                        return default(T);

                    Func<T> fact = this.GetConstructor<T>();

                    entity = fact();
                    Deserialize(entity, document);

                    _cache.Add(key, entity);
                }

                return entity;

            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// Gets a constructor for a given type. Constructor is cached for performance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Func<T> GetConstructor<T>()
        {
            Type type = typeof(T);
            string key = string.Format("inst:{0}",typeof(T).FullName);
            Func<T> func = Cache.Instance.Get(key) as Func<T>;

            if (func == null)
            {
                var method = new DynamicMethod("DM$OBJ_FACTORY_" + type.Name, type, null, type);
                ILGenerator generator = method.GetILGenerator();
                generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
                generator.Emit(OpCodes.Ret);
                func = (Func<T>)method.CreateDelegate(typeof(Func<T>));

                Cache.Instance.Add(key, func);
            }

            return func;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CollectionExists(string name)
        {
            try
            {
                return this.Database.CollectionExists(name);
            }
            catch (TimeoutException ex)
            {
                throw new MongoUnavailableException(ex);
            }
            catch (MongoConnectionException ex)
            {
                throw new MongoUnavailableException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void CreateCollection(string name) 
        {
            try
            {
                this.Database.CreateCollection(name);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<T> GetCollection<T>(string name) 
        {
            try
            {
                return this.Database.GetCollection<T>(name);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection GetCollection(string name)
        {
            try
            {
                return this.Database.GetCollection(name);
            }
            catch (MongoConnectionException)
            {
                throw new MongoUnavailableException();
            }
        }
    }

    #endregion
}
