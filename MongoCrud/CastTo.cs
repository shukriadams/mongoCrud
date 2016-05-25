using MongoDB.Bson;
using System;
using System.Text.RegularExpressions;

namespace MongoCrud
{
    public static class CastTo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool Boolean(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return false;

            if (doc[item] == BsonNull.Value)
                return false;

            return doc[item].AsBoolean;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static byte[] ByteArray(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            return doc[item].AsByteArray;
        }

        public static Regex Regex(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            return doc[item].AsRegex;
        }

        public static bool? NullableBoolean(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            return doc[item].AsNullableBoolean;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string String(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            return doc[item].AsString;
        }


        /// <summary>
        /// Converts ticks to date
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static DateTime DateTime(BsonDocument doc, string item)
        {
            return new DateTime(Int64.Parse(doc[item].ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <param name="defaulvalue"></param>
        /// <returns></returns>
        public static Int64 Long(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return 0;

            if (doc[item] == BsonNull.Value)
                return 0;

            // having to read if int32 is is ugly workaround to mongodriver's prissy inability to cast int32 as long
            if (doc[item].IsInt32)
                return doc[item].AsInt32;

            return doc[item].AsInt64;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static double? NullableDouble(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            // having to read if int32 is is ugly workaround to mongodriver's prissy inability to cast int32 as long
            if (doc[item].IsInt32)
                return doc[item].AsNullableInt32;

            return doc[item].AsNullableDouble;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <param name="defaulvalue"></param>
        /// <returns></returns>
        public static double Double(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return 0;

            if (doc[item] == BsonNull.Value)
                return 0;

            return doc[item].AsDouble;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <param name="defaulvalue"></param>
        /// <returns></returns>
        public static Int32 Int(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return 0;

            if (doc[item] == BsonNull.Value)
                return 0;

            return doc[item].AsInt32;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Int64? NullableLong(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            // having to read if int32 is is ugly workaround to mongodriver's prissy inability to cast int32 as long
            if (doc[item].IsInt32)
                return doc[item].AsNullableInt32;

            return doc[item].AsNullableInt64;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static Int32? NullableInt(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            return doc[item].AsNullableInt32;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static DateTime? NullableDateTime(BsonDocument doc, string item)
        {
            if (!doc.Contains(item))
                return null;

            if (doc[item] == BsonNull.Value)
                return null;

            return new DateTime(doc[item].AsInt64);
        }
    }
}
