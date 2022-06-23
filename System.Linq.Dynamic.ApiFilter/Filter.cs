using System.Collections.Generic;

namespace System.Linq.Dynamic.ApiFilter
{
#pragma warning disable 1591
    public sealed class Filter
    {
        public struct Operands
        {
            public const string Like = "~";
            public const string Equal = ":";
            // see issue#4 check for null and not null values
            public const string NotEqual = "<>";

            public const string GreaterThan = ">";
            public const string GreaterThanOrEqual = ">:";
            public const string SmallerThan = "<";
            public const string SmallerThanOrEqual = "<:";
            // special case. Inclusive or equal. field:(value1, value2 ...). In Linq.Dynamic -> . Where("field == @0 || field == @1 ...")
            public const string InclusiveOrEqual = ":()";
            // special case. Inclusive or like. field~(value1, value2 ...). In Linq.Dynamic -> .Where("field.Contains(@0) || field.Contains(@1) ...")
            public const string InclusiveOrLike = "~()";
            // special case. Inclusive and like. field~(value1, value2 ...). In Linq.Dynamic -> .Where("field.Contains(@0) && field.Contains(@1) ...")
            public const string InclusiveAndLike = "~:()";
        }

        private const char apostrohpe = '\'';

        private static readonly string[] _operands =
        {
            Operands.NotEqual, 
            Operands.GreaterThanOrEqual, 
            Operands.SmallerThanOrEqual, 
            Operands.GreaterThan, 
            Operands.SmallerThan, 
            Operands.Equal, 
            Operands.Like
        };

        private Filter(string name, string op, params string[] values)
        {
            Name = name;
            Operand = op;
            Values = values;
        }

        private Filter(IList<Filter> orFilters)
        {
            OrFilters = orFilters;
        }

        public string Name { get; set; }
        public string Operand { get; }
        public string[] Values { get; }
        public IList<Filter> OrFilters { get; }


        public static List<Filter> Parse(string filterValues)
        {
            var result = new List<Filter>();
            var andParts = filterValues?.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            foreach (var filter in andParts)
            {
                var orParts = filter.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (orParts.Length > 1)
                {
                    var orFilters = new List<Filter>();
                    foreach(var orFilter in orParts)
                    {
                        CreateFilter(orFilters, orFilter);
                    }
                    result.Add(new Filter(orFilters));
                }
                else
                {
                    CreateFilter(result, filter);
                }
                
            }
            return result;
        }

        private static void CreateFilter(List<Filter> result, string filter)
        {
            var op = GetOperand(filter);
            if (op == Operands.InclusiveOrEqual)
            {
                // NOTE: special case
                var args = filter.Split(new string[] { ":(", ")" }, StringSplitOptions.None);
                var name = args[0];
                var values = args[1].Split(',')
                                .Select(s => s.Trim())
                                .Select(s => RemoveApostrophes(s))
                                .ToArray();

                result.Add(new Filter(name, op, values));
            }
            else if (op == Operands.InclusiveAndLike)
            {
                // NOTE: special case
                var args = filter.Split(new string[] { "~:(", ")" }, StringSplitOptions.None);
                var name = args[0];
                var values = args[1].Split(',')
                                .Select(s => s.Trim())
                                .Select(s => RemoveApostrophes(s))
                                .ToArray();

                result.Add(new Filter(name, op, values));
            }
            else if (op == Operands.InclusiveOrLike)
            {
                // NOTE: special case
                var args = filter.Split(new string[] { "~(", ")" }, StringSplitOptions.None);
                var name = args[0];
                var values = args[1].Split(',')
                                .Select(s => s.Trim())
                                .Select(s => RemoveApostrophes(s))
                                .ToArray();

                result.Add(new Filter(name, op, values));
            }
            else
            {
                var args = filter.Split(new string[] { op }, StringSplitOptions.None);
                var name = args[0];
                var value = RemoveApostrophes(args[1]);

                result.Add(new Filter(name, op, value));
            }
        }

        // Remove surrounding apostrophe characters
        static string RemoveApostrophes(string value)
        {
            if (value.Length == 0)
            {
                return value;
            }

            if (value.IndexOf(apostrohpe) == 0)
            {
                value = value.Substring(1);
            }
            if (value.LastIndexOf(apostrohpe) == value.Length - 1)
            {
                value = value.Substring(0, value.Length - 1);
            }
            return value;
        }

        static string GetOperand(string filter)
        {
            if (filter.IndexOf("~:(") > 0 && filter.LastIndexOf(")") == filter.Length - 1)
            {
                // NOTE: special case
                return Operands.InclusiveAndLike;
            }
            if (filter.IndexOf(":(") > 0 && filter.LastIndexOf(")") == filter.Length - 1)
            {
                // NOTE: special case
                return Operands.InclusiveOrEqual;
            }
            if (filter.IndexOf("~(") > 0 && filter.LastIndexOf(")") == filter.Length - 1)
            {
                // NOTE: special case
                return Operands.InclusiveOrLike;
            }

            foreach (var op in _operands)
            {
                if (filter.IndexOf(op) > -1)
                {
                    return op;
                }
            }

            throw new DynamicFilterException(Properties.Resources.UnsupportedOperandException);
        }
    }
#pragma warning restore 1591
}

