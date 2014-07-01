using System;

namespace Exercise3.Builders
{
    public class RequerimentAttribute : Attribute
    {
        public readonly Type Type;

        public RequerimentAttribute(Type type)
        {
            Type = type;
        }
    }
}