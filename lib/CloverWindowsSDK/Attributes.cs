using System;
using System.Reflection;

namespace com.clover.remotepay.sdk
{
    [System.AttributeUsage (System.AttributeTargets.Property | System.AttributeTargets.Field)]
    public class TypeInfo : System.Attribute
    {
        public TypeInfo(Type type)
        {
            DataType = type;
        }
        public Type DataType { get; set; }
    }
}