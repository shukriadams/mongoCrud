using NUnit.Framework;
using System;
using System.Configuration;

namespace MongoCrud.Test
{
    [TestFixture]
    public class Version2ConnectionTests
    {
        private MongoDb _mongo;

        [SetUp]
        public void SetUp()
        {
            _mongo = new MongoDb(new MongoConfiguration
            {
                DatabaseName = ConfigurationManager.AppSettings["mongov2databaseName"],
                DatabaseUsername = ConfigurationManager.AppSettings["mongov2databaseUser"],
                DatabasePassword = ConfigurationManager.AppSettings["mongov2databasePassword"],
                Host = ConfigurationManager.AppSettings["mongov2databaseHost"],
                Port =Int32.Parse(ConfigurationManager.AppSettings["mongov2databasePort"])
            });

            MongoUtilities u = new MongoUtilities(_mongo);
            u.InitializeDatabase();
        }

        [TearDown]
        public void TearDown()
        {
            MongoUtilities u = new MongoUtilities(_mongo);
            u.TruncateAllCollections();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateAndRead()
        {
            // do
            TestType insert = new TestType
            {
                Id = Guid.NewGuid().ToString()
            };
            _mongo.Insert(insert);
            TestType retrieve = _mongo.GetById<TestType>(insert.Id);

            // assert
            Assert.IsNotNull(retrieve);
        }
    }
}
