using System;

namespace MongoCrud
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DataPropertyAttribute : Attribute
    {
        public string TargetProperty { get; private set; }

        public bool IsIndex { get; private set; }

        public DataPropertyAttribute()
        {

        }

        public DataPropertyAttribute(string targetProperty)
        {
            this.TargetProperty = targetProperty;
        }

        public DataPropertyAttribute(bool isIndex)
        {
            if (!isIndex)
                throw new Exception("isIndex must be true.");

            this.IsIndex = true;
            this.TargetProperty = "_id";
        }

    }
}
 