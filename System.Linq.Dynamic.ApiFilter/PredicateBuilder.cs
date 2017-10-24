using System.Reflection;

namespace System.Linq.Dynamic.ApiFilter
{

    public abstract class PredicateBuilder<TEntity> : IPredicateBuilder
    {

        protected const string ArrayPredicateFormat = "{0}.Any({1})";

        protected readonly Filter Filter;

        protected PredicateBuilder(Filter filter)
        {
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }


        public string Build(object[] values)
        {
            string propName = null;
            string result = null;
            var propArray = Filter.Name.Split('.');
            var op = Filter.Operand;
            try
            {
                if (propArray[0].IsArrayOrCollection<TEntity>())
                {
                    // we only support 2 levels on array elements
                    if (propArray.Length != 2)
                        // TODO: custom exception
                        throw new DynamicFilterException(Properties.Resources.UnsupportedArrayArgLength);

                    propName = propArray[1];
                    result = string.Format(ArrayPredicateFormat, propArray[0], GetPropertyFormat(propName, op, values));
                }
                else
                {
                    propName = Filter.Name;
                    result = GetPropertyFormat(propName, op, values);
                }
            }
            catch (NotSupportedException ex)
            {
                throw new DynamicFilterException(string.Format(Properties.Resources.OperandForAttributeNotSupportedException, op, propName), ex);
            }

            return result;
        }

        protected abstract string GetPropertyFormat(string property, string op, params object[] values);

    }
}

