namespace System.Linq.Dynamic.ApiFilter
{
    public class DefaultPredicateBuilder<TEntity> : PredicateBuilder<TEntity>
    {
        private const string NotEqualsPredicateFormat = "{0} != @0";
        private const string EqualsPredicateFormat = "{0} == @0";
        private const string GreaterThanPredicateFormat = "null != {0} && {0} > @0";
        private const string GreaterThanOrEqualsPredicateFormat = "null != {0} && {0} >= @0";
        private const string SmallerThanPredicateFormat = "null != {0} && {0} < @0";
        private const string SmallerThanOrEqualsPredicateFormat = "null != {0} && {0} <= @0";

        private const string InclusiveOrEqualPredicateFormat = "{0} == {1}";


        public DefaultPredicateBuilder(Filter filter) : base(filter)
        {
        }

        protected override string GetPropertyFormat(string property, string op, params object[] values)
        {
            switch (op)
            {
                case Filter.Operands.NotEqual:
                    return string.Format(NotEqualsPredicateFormat, property);
                case Filter.Operands.Equal:
                    return string.Format(EqualsPredicateFormat, property);
                case Filter.Operands.GreaterThan:
                    return string.Format(GreaterThanPredicateFormat, property);
                case Filter.Operands.GreaterThanOrEqual:
                    return string.Format(GreaterThanOrEqualsPredicateFormat, property);
                case Filter.Operands.SmallerThan:
                    return string.Format(SmallerThanPredicateFormat, property);
                case Filter.Operands.SmallerThanOrEqual:
                    return string.Format(SmallerThanOrEqualsPredicateFormat, property);

                case Filter.Operands.InclusiveOrEqual:

                    var result = "";
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i > 0)
                        {
                            result += " || ";
                        }

                        result += string.Format(InclusiveOrEqualPredicateFormat, "@" + i.ToString(), property);
                    }
                    return result;

                default:
                    throw new NotSupportedException(op);
            }
        }
    }
}

