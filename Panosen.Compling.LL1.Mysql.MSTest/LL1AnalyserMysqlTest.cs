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

            var tokenCollection = new GrammarTokenizer(grammar.Rules).Analyze("select a, b from book limit 1,2;");

            Assert.AreEqual(11, tokenCollection.Count());

            GrammarNode root;
            Token errorToken;

            var accept = LL1Analyser.Analyse(tokenCollection, out root, out errorToken, grammar);

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new TINYNodeConvertor());

            File.WriteAllText(@"f:\tmp007\2.json", JsonConvert.SerializeObject(root, Formatting.Indented, settings));

            Assert.IsTrue(accept);
        }
    }
}
