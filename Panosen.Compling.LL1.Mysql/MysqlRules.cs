using System;
using System.Collections.Generic;

namespace Panosen.Compling.LL1.Mysql
{
    public class MysqlRules
    {
        public List<ProductionRule> GetRules()
        {
            List<ProductionRule> rules = new List<ProductionRule>();
            rules.Add(ToRule("TEXT", "STATEMENT", "STATEMENTS_RIGHT"));
            rules.Add(ToRule("STATEMENTS_RIGHT", "SEMICOLONS", "STATEMENT", "STATEMENTS_RIGHT"));
            rules.Add(ToRule("STATEMENTS_RIGHT", "[Null]"));
            rules.Add(ToRule("STATEMENT", "SELECT_STATEMENT"));
            rules.Add(ToRule("STATEMENT", "UPDATE_STATEMENT"));
            rules.Add(ToRule("STATEMENT", "CREATE_STATEMENT"));
            rules.Add(ToRule("STATEMENT", "DELETE_STATEMENT"));

            // ;+
            rules.Add(ToRule("SEMICOLONS", ";", "SEMICOLONS_RIGHT"));
            rules.Add(ToRule("SEMICOLONS_RIGHT", ";", "SEMICOLONS_RIGHT"));
            rules.Add(ToRule("SEMICOLONS_RIGHT", "[Null]"));

            // <select><from><where>
            rules.Add(ToRule("SELECT_STATEMENT", "SELECT_PART", "FROM_PART", "WHERE_PART", "LIMIT_PART"));

            rules.Add(ToRule("UPDATE_STATEMENT", "update"));

            rules.Add(ToRule("CREATE_STATEMENT", "create"));

            rules.Add(ToRule("DELETE_STATEMENT", "delete"));

            rules.Add(ToRule("SELECT_PART", "select", "COLUMNS"));
            rules.Add(ToRule("COLUMNS", "IDENTIFIER", "COLUMNS_RIGHT"));
            rules.Add(ToRule("COLUMNS_RIGHT", ",", "IDENTIFIER", "COLUMNS_RIGHT"));
            rules.Add(ToRule("COLUMNS_RIGHT", "[Null]"));

            rules.Add(ToRule("FROM_PART", "from", "TABLE"));
            rules.Add(ToRule("TABLE", "book"));
            rules.Add(ToRule("TABLE", "user"));
            rules.Add(ToRule("FROM_PART", "[Null]"));

            rules.Add(ToRule("WHERE_PART", "where"));
            rules.Add(ToRule("WHERE_PART", "[Null]"));

            rules.Add(ToRule("LIMIT_PART", "limit", "NUMBERS", "LIMIT_SIZE"));
            rules.Add(ToRule("LIMIT_PART", "[Null]"));

            rules.Add(ToRule("LIMIT_SIZE", ",", "NUMBERS"));
            rules.Add(ToRule("LIMIT_SIZE", "[Null]"));

            rules.Add(ToRule("LIMIT_FROM_SIZE", "NUMBERS"));

            rules.Add(ToRule("NUMBERS", "NUMBER", "NUMBER_RIGHT"));
            rules.Add(ToRule("NUMBER_RIGHT", "NUMBER", "NUMBER_RIGHT"));
            rules.Add(ToRule("NUMBER_RIGHT", "[Null]"));

            //标志符
            rules.Add(ToRule("IDENTIFIER", "UNDERLINE", "IDENTIFIER_RIGHT"));
            rules.Add(ToRule("IDENTIFIER", "LETTER", "IDENTIFIER_RIGHT"));
            rules.Add(ToRule("IDENTIFIER_RIGHT", "UNDERLINE_LETTER_NUMBER", "IDENTIFIER_RIGHT"));
            rules.Add(ToRule("IDENTIFIER_RIGHT", "[Null]"));

            rules.Add(ToRule("UNDERLINE_LETTER_NUMBER", "LETTER"));
            rules.Add(ToRule("UNDERLINE_LETTER_NUMBER", "NUMBER"));
            rules.Add(ToRule("UNDERLINE_LETTER_NUMBER", "UNDERLINE"));

            //字母
            for (int index = 'a'; index <= 'z'; index++)
            {
                rules.Add(ToRule("LETTER", ((char)index).ToString()));
            }
            for (int index = 'A'; index <= 'Z'; index++)
            {
                rules.Add(ToRule("LETTER", ((char)index).ToString()));
            }
            //数字
            for (int index = '0'; index <= '9'; index++)
            {
                rules.Add(ToRule("NUMBER", ((char)index).ToString()));
            }
            //下划线
            rules.Add(ToRule("UNDERLINE", "_"));

            return rules;
        }

        private static ProductionRule ToRule(params string[] items)
        {
            ProductionRule theRule = new ProductionRule();
            theRule.Left = new Symbol { Value = items[0], Type = GetSymbolType(items[0]) };

            theRule.Right = new List<Symbol>();
            for (int i = 1; i < items.Length; i++)
            {
                theRule.Right.Add(new Symbol { Value = items[i], Type = GetSymbolType(items[i]) });
            }

            return theRule;
        }

        private static SymbolType GetSymbolType(string item)
        {
            if (item == "[Null]")
            {
                return SymbolType.Epsilon;
            }

            if (item.Length == 1)
            {
                return SymbolType.Terminal;
            }

            foreach (var ch in item)
            {
                if (ch == '_')
                {
                    continue;
                }
                if ('a' <= ch && ch <= 'z')
                {
                    return SymbolType.Terminal;
                }
            }

            return SymbolType.NonTerminal;
        }
    }
}
