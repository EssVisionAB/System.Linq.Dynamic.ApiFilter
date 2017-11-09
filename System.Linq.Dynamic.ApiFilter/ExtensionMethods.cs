using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace System.Linq.Dynamic.ApiFilter
{
    static class ExtensionMethods
    {

        public static readonly BindingFlags IngoreCaseFlags =
            BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

        public static object[] ConvertFromStringValues(this string[] source, Type targetType)
        {
            if (null != targetType)
            {
                var converter = TypeDescriptor.GetConverter(targetType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    var result = new ArrayList();
                    foreach (var s in source)
                    {
                        if(s.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(null);
                            continue;
                        }

                        var value = converter.ConvertFromString(s);
                        result.Add(value);
                    }
                    return result.ToArray();
                }
            }
            return source.Cast<object>().ToArray();
        }


        public static Type GetTargetType<TEntity>(this Filter filter)
        {
            var propType = typeof(TEntity);
            var propArray = filter.Name.Split('.');
            foreach (var name in propArray)
            {
                if (propType.IsArrayOrCollection())
                {
                    // TODO: check if type has generic argument otherwise get element type
                    if (propType.IsGenericType)
                    {
                        propType = propType.GenericTypeArguments[0];
                    }
                    else if (propType.IsArray)
                    {
                        propType = propType.GetElementType();
                    }
                }
                var p = propType.GetProperty(name, IngoreCaseFlags);
                if (null == p)
                    throw new DynamicFilterException(string.Format(Properties.Resources.InvalidFilterAttributeException, name));

                propType = p.PropertyType;
            }

            return propType;
        }

        public static bool IsArrayOrCollection<TEntity>(this string property)
        {
            var p = typeof(TEntity).GetProperty(property, IngoreCaseFlags);
            if (null == p)
                throw new DynamicFilterException(string.Format(Properties.Resources.InvalidFilterAttributeException, property));

            return p.PropertyType.IsArrayOrCollection();

        }

        public static bool IsArrayOrCollection(this Type type)
        {
            return type != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(type));
        }


        public static string NullPropName(this string value)
        {
            if (value.IndexOf('.') > 0)
            {
                // return base property name 
                return value.Split('.')[0];
            }

            return value;
        }
    }
}
