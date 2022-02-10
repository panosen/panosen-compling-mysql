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
            var productionRules = new ProductionRuleCollection(new MysqlRules().GetRules());

            Grammar grammar = new Grammar();
            grammar.Rules = productionRules.ProductionRules;

            var tokenCollection = new TokenCollection();
            tokenCollection.AddToken("select");
            tokenCollection.AddToken("a");
            tokenCollection.AddToken(",");
            tokenCollection.AddToken("b");
            tokenCollection.AddToken("from");
            tokenCollection.AddToken("book");
            tokenCollection.AddToken("limit");
            tokenCollection.AddToken("1");
            tokenCollection.AddToken(",");
            tokenCollection.AddToken("2");
            tokenCollection.AddToken(";");

            Assert.AreEqual(11, tokenCollection.Count());

            GrammarNode root;
            Token errorToken;

            var accept = LL1Analyser.Analyse(tokenCollection, out root, out errorToken, grammar);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new TINYNodeConvertor());

            Assert.IsTrue(accept);
        }
    }
}
