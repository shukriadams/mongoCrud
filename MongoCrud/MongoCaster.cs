using System;
using MongoDB.Bson;

namespace MongoCrud
{
    /// <summary>
    /// Helper class for handling Mongo's C# casting problems.
    /// </summary>
    public class MongoCaster
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static BsonValue TypeNameOrNull(Type type)
        {
            if (type == null)
                return BsonNull.Value;
            return type.FullName;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue ValueOrNull(string value)
        {
            if (string.IsNullOrEmpty(value))
                return BsonNull.Value;
            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static BsonValue TicksOrNull(DateTime? dateTime) 
        {
            if (dateTime.HasValue)
                return dateTime.Value.Ticks;
            return BsonNull.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue ValueOrNull(int? value)
        {
            if (value == null)
                return BsonNull.Value;
            return value.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue ValueOrNull(long? value)
        {
            if (value == null)
                return BsonNull.Value;
            return value.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue ValueOrNull(bool? value)
        {
            if (value == null)
                return BsonNull.Value;
            return value.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue ValueOrNull(DateTime? value)
        {
            if (value == null)
                return BsonNull.Value;
            return value.Value.Ticks;
        }

    }
}
