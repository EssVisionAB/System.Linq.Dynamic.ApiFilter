using System.Collections.Generic;
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
            if(null != Filter.OrFilters)
            {
                var filters = new List<string>();
                foreach(var filter in Filter.OrFilters)
                {
                    filters.Add(BuildFilter(filter, values));
                }

                return string.Join(" || ", filters);
            }

            return BuildFilter(Filter, values);
        }

        private string BuildFilter(Filter filter, object[] values)
        {
            string propName = null;
            var propArray = filter.Name.Split('.');
            var op = filter.Operand;
            string result;
            try
            {
                if (propArray[0].IsArrayOrCollection<TEntity>())
                {
                    // NOTE: Special case for primitive string arrays
                    // TODO: Check that source type is string[]
                    if(propArray.Length == 1 && (op == Filter.Operands.Equal || op == Filter.Operands.Like))
                    {
                        if(op == Filter.Operands.Like)
                        {
                            result = $"{propArray[0]}.Any(s => s.Contains(\"{values[0]}\"))";
                        }
                        else if (op == Filter.Operands.Equal)
                        {
                            result = $"{propArray[0]}.Contains(\"{values[0]}\")";
                        }
                        else
                        {
                            throw new DynamicFilterException($"Operand {op} is not supported for primitive string arrays");
                        }
                    }
                    else
                    {
                        // we only support 2 levels on array elements
                        if (propArray.Length != 2)
                        {
                            // TODO: custom exception
                            throw new DynamicFilterException(Properties.Resources.UnsupportedArrayArgLength);
                        }

                        propName = propArray[1];
                        result = string.Format(ArrayPredicateFormat, propArray[0], GetPropertyFormat(propName, op, values));
                    }
                }
                else
                {
                    propName = filter.Name;
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

