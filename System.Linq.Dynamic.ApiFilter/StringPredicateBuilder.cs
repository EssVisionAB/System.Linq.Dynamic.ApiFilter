namespace System.Linq.Dynamic.ApiFilter
{
    public class StringPredicateBuilder<TEntity> : PredicateBuilder<TEntity>
    {

        private const string __LikePredicateFormat = "{0}.Contains(@0)";
        private const string __EqualsPredicateFormat = "{0} == @0";
        private const string __NotEqualsPredicateFormat = "{0} != @0";

        private const string __InclusiveOrEqualPredicateFormat = "{0} == {1}";
        private const string __InclusiveOrLikePredicateFormat = "{1}.Contains({0})";


        protected virtual string LikePredicateFormat
        {
            get { return __LikePredicateFormat; }
        }

        protected virtual string EqualsPredicateFormat
        {
            get { return __EqualsPredicateFormat; }
        }

        protected virtual string NotEqualsPredicateFormat
        {
            get { return __NotEqualsPredicateFormat; }
        }

        protected virtual string InclusiveOrEqualPredicateFormat
        {
            get { return __InclusiveOrEqualPredicateFormat; }
        }

        protected virtual string InclusiveOrLikePredicateFormat
        {
            get { return __InclusiveOrLikePredicateFormat; }
        }

        public StringPredicateBuilder(Filter filter) : base(filter)
        {
        }

        protected override string GetPropertyFormat(string property, string op, params object[] values)
        {

            switch (op)
            {
                case Filter.Operands.Like:
                    return string.Format(LikePredicateFormat, property);

                case Filter.Operands.Equal:
                    return string.Format(EqualsPredicateFormat, property);

                case Filter.Operands.NotEqual:
                    return string.Format(NotEqualsPredicateFormat, property);

                case Filter.Operands.InclusiveOrEqual:
                    {
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
                    }
                case Filter.Operands.InclusiveOrLike:
                    {
                        var result = "";
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (i > 0)
                            {
                                result += " || ";
                            }

                            result += string.Format(InclusiveOrLikePredicateFormat, "@" + i.ToString(), property);
                        }
                        return result;
                    }

                default:
                    throw new NotSupportedException(op);
            }
        }
    }
}

