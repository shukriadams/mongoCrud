using NUnit.Framework;
using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace MongoCrud.Test
{
    [TestFixture]
    public class CRUDTests
    {
        private MongoDb _mongo;

        [SetUp]
        public void SetUp()
        {
            string connectionString = ConfigurationManager.AppSettings["mongoConnectionString"];

            _mongo = new MongoDb(connectionString, ConfigurationManager.AppSettings["mongoDatabase"]);
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
        public void CreateAndReadBasic()
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
            Assert.AreEqual(retrieve.Id, insert.Id);
            Assert.AreEqual(retrieve.Bool, false);
            Assert.AreEqual(retrieve.BoolNull, null);
            Assert.AreEqual(retrieve.ByteArray, null);
            Assert.AreEqual(retrieve.DateTime, default(DateTime));
            Assert.AreEqual(retrieve.DateTimeNull, null);
            Assert.AreEqual(retrieve.Double, 0);
            Assert.AreEqual(retrieve.DoubleNull, null);
            Assert.AreEqual(retrieve.Int64, 0);
            Assert.AreEqual(retrieve.Int64Null, null);
            Assert.AreEqual(retrieve.Regex, null);
            Assert.AreEqual(retrieve.String, null);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateWithValues()
        {
            // do
            TestType insert = new TestType
            {
                Id = Guid.NewGuid().ToString(),
                Bool = true,
                BoolNull = false,
                ByteArray = Encoding.Default.GetBytes("some bytes"),
                DateTime = new DateTime(2001, 1, 1),
                DateTimeNull = new DateTime(2002, 1, 1),
                Double = Double.MaxValue,
                DoubleNull = Double.MinValue,
                Int32 = int.MaxValue,
                Int32Null = int.MinValue,
                Int64 = long.MaxValue,
                Int64Null = long.MinValue,
                Regex = new Regex(@"^\d$"),
                String = "some string"
            };
            _mongo.Insert(insert);
            TestType retrieve = _mongo.GetById<TestType>(insert.Id);
            

            // assert
            Assert.IsNotNull(retrieve);
            Assert.AreEqual(retrieve.Bool, true);
            Assert.AreEqual(retrieve.BoolNull, false);
            Assert.AreEqual(retrieve.ByteArray, Encoding.Default.GetBytes("some bytes"));
            Assert.AreEqual(retrieve.DateTime, new DateTime(2001, 1, 1));
            Assert.AreEqual(retrieve.DateTimeNull, new DateTime(2002, 1, 1));
            Assert.AreEqual(retrieve.Double, Double.MaxValue);
            Assert.AreEqual(retrieve.DoubleNull, Double.MinValue);
            Assert.AreEqual(retrieve.Int32, int.MaxValue);
            Assert.AreEqual(retrieve.Int32Null, int.MinValue);
            Assert.AreEqual(retrieve.Int64, long.MaxValue);
            Assert.AreEqual(retrieve.Int64Null, long.MinValue);
            Assert.AreEqual(retrieve.Regex.ToString(), new Regex(@"^\d$").ToString()); // toString because Nunit fails when comparing Regexs
            Assert.AreEqual(retrieve.String, "some string");
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateWithValuesThenUpdate()
        {
            // do
            TestType insert = new TestType
            {
                Id = Guid.NewGuid().ToString(),
                Bool = true,
                BoolNull = false,
                ByteArray = Encoding.Default.GetBytes("some bytes"),
                DateTime = new DateTime(2001, 1, 1),
                DateTimeNull = new DateTime(2002, 1, 1),
                Double = Double.MaxValue,
                DoubleNull = Double.MinValue,
                Int32 = int.MaxValue,
                Int32Null = int.MinValue,
                Int64 = long.MaxValue,
                Int64Null = long.MinValue,
                Regex = new Regex(@"^\d$"),
                String = "some string"
            };
            _mongo.Insert(insert);
            TestType update = _mongo.GetById<TestType>(insert.Id);
            update.Bool = false;
            update.BoolNull = null;
            update.ByteArray = Encoding.Default.GetBytes("some other bytes");
            update.DateTime = new DateTime(2001, 2, 2);
            update.DateTimeNull = null;
            update.Double = Double.MinValue;
            update.DoubleNull = null;
            update.Int32 = int.MinValue;
            update.Int32Null = null;
            update.Int64 = long.MinValue;
            update.Int64Null = null;
            update.Regex = new Regex("");
            update.String = "some other string";

            _mongo.Update(update);
            TestType retrieve = _mongo.GetById<TestType>(insert.Id);


            // assert
            Assert.IsNotNull(retrieve);
            Assert.AreEqual(retrieve.Bool, false);
            Assert.AreEqual(retrieve.BoolNull, null);
            Assert.AreEqual(retrieve.ByteArray, Encoding.Default.GetBytes("some other bytes"));
            Assert.AreEqual(retrieve.DateTime, new DateTime(2001, 2, 2));
            Assert.AreEqual(retrieve.DateTimeNull, null);
            Assert.AreEqual(retrieve.Double, Double.MinValue);
            Assert.AreEqual(retrieve.DoubleNull, null);
            Assert.AreEqual(retrieve.Int32, int.MinValue);
            Assert.AreEqual(retrieve.Int32Null, null);
            Assert.AreEqual(retrieve.Int64, long.MinValue);
            Assert.AreEqual(retrieve.Int64Null, null);
            Assert.AreEqual(retrieve.Regex.ToString(), new Regex("").ToString());
            Assert.AreEqual(retrieve.String, "some other string");
        }


        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void CreateAndDelete()
        {
            // do
            TestType insert = new TestType
            {
                Id = Guid.NewGuid().ToString()
            };
            _mongo.Insert(insert);
            _mongo.Delete<TestType>(insert.Id);
            TestType retrieve = _mongo.GetById<TestType>(insert.Id);

            Assert.IsNull(retrieve);
        }

        
        /// <summary>
        /// Confirm that attempting to connect to an invalid database throws custom exception
        /// </summary>
        [Test]
        public void DatabaseOffline()
        {
            MongoDb mongo = new MongoDb(new MongoConfiguration {
                DatabaseName = "invalidDatabaseName",
                DatabasePassword = "mongocrud",
                DatabaseUsername = "mongocrud",
                Host = "127.0.0.2",
                Port = 27017
            });

            try
            {
                TestType retrieve = mongo.GetById<TestType>("some id");
                Assert.Fail("Expected exception not thrown");
            }
            catch (MongoUnavailableException)
            {
                Assert.Pass("Expected exception thrown");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception thrown");
            }
            
        }
    }


}
