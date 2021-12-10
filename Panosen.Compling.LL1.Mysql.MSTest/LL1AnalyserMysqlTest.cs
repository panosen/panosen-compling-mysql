using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Panosen.Compling.LL1.Mysql.MSTest
{
    [TestClass]
    public class LL1AnalyserMysqlTest
    {
        [TestMethod]
        public void TestIsLL1Grammar()
        {
            Grammar grammar = new Grammar();
            grammar.Rules = new MysqlRules().GetRules();

            var isLL1Grammar = grammar.IsLL1Grammar();
            Assert.IsTrue(isLL1Grammar);
        }

        [TestMethod]
        public void TestSelectPart()
        {
            Grammar grammar = new Grammar();
            grammar.Rules = new MysqlRules().GetRules();

            var tokenList = new List<Symbol>();
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "select" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "a" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "," });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "b" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "from" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "book" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "limit" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "1" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "," });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = "2" });
            tokenList.Add(new Symbol { Type = SymbolType.Terminal, Value = ";" });

            GrammarNode root;
            Symbol errorToken;

            var accept = LL1Analyser.Analyse(tokenList, out root, out errorToken, grammar);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new TINYNodeConvertor());

            File.WriteAllText(@"f:\tmp007\2.json", JsonConvert.SerializeObject(root, Formatting.Indented, settings));

            Assert.IsTrue(accept);
        }
    }
}
