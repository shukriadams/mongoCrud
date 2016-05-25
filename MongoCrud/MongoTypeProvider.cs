using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoCrud
{
    public static class MongoTypeProvider
    {
        private static MongoDefine _instance;

        public static MongoDefine GetDefinition()
        {
            if (_instance == null)
            {
                IEnumerable<Type> types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                           from assemblyType in assembly.GetTypes()
                                           where typeof(MongoDefine).IsAssignableFrom(assemblyType)
                                               && !assemblyType.IsAbstract
                                           select assemblyType).ToArray();

                if (types.Count() == 0)
                    throw new Exception(string.Format("No types of {0} found, please declare one to define your database.", typeof(MongoDefine).Name));

                if (types.Count() > 1)
                    throw new Exception(string.Format("Multiple types of {0} found, only 1 type is allowed per application", typeof(MongoDefine).Name));

                _instance = Activator.CreateInstance(types.First()) as MongoDefine;
            }

            return _instance;
        }
    }
}
