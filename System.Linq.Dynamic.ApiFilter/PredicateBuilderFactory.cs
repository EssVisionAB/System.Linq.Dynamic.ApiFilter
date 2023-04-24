using System.Collections.Generic;
using System.Reflection;

namespace System.Linq.Dynamic.ApiFilter
{
    public class PredicateBuilderFactory : IPredicateBuilderFactory
    {

        protected IDictionary<string,Type> BuilderTypes
        {
            get;
        }


        public PredicateBuilderFactory()
        {
            BuilderTypes = new Dictionary<string, Type>();

            // add default string predicate builder used in sql queris
            BuilderTypes.Add(typeof(string).FullName, typeof(StringPredicateBuilder<>));

        }

        public void AddBuilderType(string targetTypeName, Type builderType)
        {
            if(!typeof(IPredicateBuilder).IsAssignableFrom(builderType))
            {
                throw new ArgumentException(string.Format("Builder type must be assignable from {0}", typeof(IPredicateBuilder)));
            }

            if (BuilderTypes.ContainsKey(targetTypeName))
            {
                BuilderTypes[targetTypeName] = builderType;
            }
            else
            {
                BuilderTypes.Add(targetTypeName, builderType);
            }
        }

        public void AddBuilderType<T>(string targetTypeName)
        {
            this.AddBuilderType(targetTypeName, typeof(T));
        }

        public IPredicateBuilder Create<TEntity>(Filter filter)
        {
            var key = filter.GetTargetType<TEntity>().FullName;

            if (BuilderTypes.ContainsKey(key))
            {
                var classType = BuilderTypes[key];
                if (classType.IsGenericType)
                {
                    classType = classType.MakeGenericType(typeof(TEntity));
                }
                try
                {
                    return (IPredicateBuilder)Activator.CreateInstance(classType, new object[] { filter });
                }
                catch (TargetInvocationException ex)
                {
                    if (null != ex.InnerException)
                        throw ex.InnerException;

                    throw;
                }
            }

            // Default
            return new DefaultPredicateBuilder<TEntity>(filter);

        }
    }
}
