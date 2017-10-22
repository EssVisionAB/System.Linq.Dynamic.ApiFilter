namespace System.Linq.Dynamic.ApiFilter
{
    public class StringPredicateBuilder<TEntity> : PredicateBuilder<TEntity>
    {

        private const string __LikePredicateFormat = "{0}.Contains(@0)";
        private const string __EqualsPredicateFormat = "{0} == @0";

        private const string __InclusiveOrPredicateFormat = "{0} == {1}";


        protected virtual string LikePredicateFormat
        {
            get { return __LikePredicateFormat; }
        }

        protected virtual string EqualsPredicateFormat
        {
            get { return __EqualsPredicateFormat; }
        }

        protected virtual string InclusiveOrPredicateFormat
        {
            get { return __InclusiveOrPredicateFormat; }
        }


        public StringPredicateBuilder(Filter filter) : base(filter)
        {
        }

        protected override string GetPropertyFormat(string property, string op, params object[] values)
        {

            switch (op)
            {
                case Filter.Operands.Like:
                    return string.Format(LikePredicateFormat, property, property.NullPropName());

                case Filter.Operands.Equal:
                    return string.Format(EqualsPredicateFormat, property, property.NullPropName());

                case Filter.Operands.InclusiveOr:

                    var result = "";
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (i > 0)
                        {
                            result += " || ";
                        }

                        result += string.Format(InclusiveOrPredicateFormat, "@" + i.ToString(), property);
                    }
                    return result;

                default:
                    throw new NotSupportedException(op);
            }
        }
    }
}

