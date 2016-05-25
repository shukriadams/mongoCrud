using System.Collections.Generic;

namespace MongoCrud
{
    public abstract class MongoDefine 
    {
        public abstract IEnumerable<MCollection> Define();
    }
}
