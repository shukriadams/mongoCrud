using MongoDB.Bson;
using System;
using System.Text.RegularExpressions;

namespace MongoCrud
{
    public static class CastFrom
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue String(object value)
        {
            string val = (string)value;
            if (value == null)
                return BsonNull.Value;
            return BsonString.Create(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue DateTime(object value)
        {
            return BsonInt64.Create(((DateTime)value).Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue NullableDateTime(object value)
        {
            DateTime? val = (DateTime?)value;
            if (val == null)
                return BsonNull.Value;

            return BsonInt64.Create(val.Value.Ticks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue Boolean(object value)
        {
            return BsonBoolean.Create((bool)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue NullableBoolean(object value)
        {
            bool? val = (bool?)value;
            if (val == null)
                return BsonNull.Value;

            return BsonBoolean.Create(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue Int(object value)
        {
            return BsonInt32.Create((int)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue NullableInt(object value)
        {
            int? val = (int?)value;
            if (val == null)
                return BsonNull.Value;

            return BsonInt32.Create(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue Long(object value)
        {
            return BsonInt64.Create((long)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue NullableLong(object value)
        {
            long? val = (long?)value;
            if (val == null)
                return BsonNull.Value;

            return BsonInt64.Create(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue Double(object value)
        {
            return BsonDouble.Create((double)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue NullableDouble(object value)
        {
            double? val = (double?)value;
            if (val == null)
                return BsonNull.Value;

            return BsonDouble.Create(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue Regex(object value)
        {
            Regex val = (Regex)value;
            if (val == null)
                return BsonNull.Value;

            return BsonRegularExpression.Create(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BsonValue ByteArray(object value)
        {
            byte[] val = (byte[])value;
            if (val == null)
                return BsonNull.Value;
            return BsonBinaryData.Create((byte[])value);
        }
    }
}
