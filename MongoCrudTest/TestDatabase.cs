using System.Collections.Generic;

namespace MongoCrud.Test
{
    public class TestDatabase : MongoDefine
    {
        public override IEnumerable<MCollection> Define()
        {
            return new List<MCollection>(){
                new MCollection("TestType"),
                new MCollection("ComplexType", new string[] { "Field1", "Field2" } )
            };
        }
    }
}
