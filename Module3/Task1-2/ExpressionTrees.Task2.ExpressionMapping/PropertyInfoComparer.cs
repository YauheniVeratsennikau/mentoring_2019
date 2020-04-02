using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    class PropertyInfoComparer : IEqualityComparer<PropertyInfo>
    {
        public bool Equals(PropertyInfo x, PropertyInfo y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Name.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase) && x.PropertyType == y.PropertyType;
        }

        public int GetHashCode(PropertyInfo property)
        {
            //Check whether the object is null
            if (ReferenceEquals(property, null))
            {
                return 0;
            }

            var hashPropertyName = string.IsNullOrEmpty(property.Name) ? 0 : property.Name.GetHashCode();

            var hashPropertyType = property.PropertyType.GetHashCode();

            return hashPropertyName ^ hashPropertyType;
        }
    }
}
