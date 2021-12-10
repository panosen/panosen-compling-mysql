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
            rules.Add(ToRule("COLUMNS", "COLUMN", "COLUMNS_RIGHT"));
            rules.Add(ToRule("COLUMNS_RIGHT", ",", "COLUMN", "COLUMNS_RIGHT"));
            rules.Add(ToRule("COLUMNS_RIGHT", "[Null]"));
            rules.Add(ToRule("COLUMN", "a"));
            rules.Add(ToRule("COLUMN", "b"));
            rules.Add(ToRule("COLUMN", "c"));

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

            rules.Add(ToRule("NUMBER", "0"));
            rules.Add(ToRule("NUMBER", "1"));
            rules.Add(ToRule("NUMBER", "2"));
            rules.Add(ToRule("NUMBER", "3"));
            rules.Add(ToRule("NUMBER", "4"));
            rules.Add(ToRule("NUMBER", "5"));
            rules.Add(ToRule("NUMBER", "6"));
            rules.Add(ToRule("NUMBER", "7"));
            rules.Add(ToRule("NUMBER", "8"));
            rules.Add(ToRule("NUMBER", "9"));

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

            foreach (var ch in item)
            {
                if (ch == '_')
                {
                    continue;
                }
                if (ch < 'A' || ch > 'Z')
                {
                    return SymbolType.Terminal;
                }
            }

            return SymbolType.NonTerminal;
        }
    }
}
