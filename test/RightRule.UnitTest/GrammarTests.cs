using RightRule.Rules;

namespace BSoftTests
{
    public class GrammarTests
    {
        private readonly Dictionary<string, object> _Data;

        public GrammarTests()
        {
            // Data
            _Data = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                { "name", "John" },
                { "surname", "Doe" },
                { "fullname", "John D. Doe" },
                { "age", 28 },
                { "numberoforders", 10 },
                { "goodpayer", true },
                { "joined", new DateTime(2020, 1, 1) } // yyyy mm dd
            };
        }

        [Fact]
        public void TestItWorks_NoDecision()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 1,
                Name = "Age Rule",
                Statement = @"age equals 28",
                Context = "AgeContext",
                Decision = "OLDENOUGH",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("AgeContext1", _Data);

            Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
        }

        [Fact]
        public void TestItWorks_Number_Equals()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 1,
                Name = "Age Rule",
                Statement = @"age equals 28",
                Context = "AgeContext",
                Decision = "OLDENOUGH",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("AgeContext", _Data);

            Assert.NotNull(appliedRule);
        }

        [Fact]
        public void TestItWorks_Number_NotEquals()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 1,
                Name = "Age Rule",
                Statement = "age notequals 18",
                Context = "AgeContext",
                Decision = "OLDENOUGH",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("AgeContext", _Data);

            Assert.Equal(appliedRule.Decision, rule.Decision);
        }

        [Fact]
        public void TestItWorks_Number_NotEqualsSymbol1()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 1,
                Name = "Age Rule",
                Statement = "age != 18",
                Context = "AgeContext",
                Decision = "OLDENOUGH",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("AgeContext", _Data);

            Assert.Equal(appliedRule.Decision, rule.Decision);
        }

        [Fact]
        public void TestItWorks_Number_NotEqualsSymbol2()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 1,
                Name = "Age Rule",
                Statement = "age <> 18",
                Context = "AgeContext",
                Decision = "OLDENOUGH",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("AgeContext", _Data);

            Assert.Equal(appliedRule.Decision, rule.Decision);
        }

        [Fact]
        public void TestItWorks_Number_EqualsSymbol()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 1,
                Name = "Age Rule",
                Statement = "age = 28",
                Context = "AgeContext",
                Decision = "OLDENOUGH",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("AgeContext", _Data);

            Assert.NotNull(appliedRule);
        }

        [Fact]
        public void TestItWorks_String_Equals()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 2,
                Name = "Name Rule",
                Statement = @"name equals ""John""",
                Context = "NameContext",
                Decision = "NAMEFOUND",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("NameContext", _Data);

            Assert.NotNull(appliedRule);
        }

        [Fact]
        public void TestItWorks_String_Equals_String()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 3,
                Name = "Name Rule",
                Statement = "name equals fullname",
                Context = "NameContext",
                Decision = "NAMESAMEASFULLNAME",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("NameContext", _Data);

            Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
        }

        [Fact]
        public void TestItWorks_Equals_Boolean_True()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 4,
                Name = "Good Payer Rule",
                Statement = "goodpayer equals true",
                Context = "GoodPayerContext",
                Decision = "GOODPAYER",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("GoodPayerContext", _Data);

            Assert.Equal(newRule.Decision, appliedRule.Decision);
        }

        [Fact]
        public void TestItWorks_Equals_Boolean_False()
        {
            // Rule
            RuleDefinition rule = new()
            {
                Number = 4,
                Name = "Good Payer Rule",
                Statement = "goodpayer equals false",
                Context = "NotGoodPayerContext",
                Decision = "NOTGOODPAYER",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("NotGoodPayerContext", _Data);

            Assert.Equal(AppliedRule.DefaultRule().Decision, appliedRule.Decision);
        }

        [Fact]
        public void TestItWorks_String_Equals_MultipleContext1()
        {
            // Rule
            RuleDefinition rule1 = new()
            {
                Number = 1,
                Name = "Name Rule",
                Statement = @"name equals ""John""",
                Context = "NAMECONTEXT1",
                Decision = "NAMEFOUND",
            };

            RuleDefinition rule2 = new()
            {
                Number = 2,
                Name = "Name Rule",
                Statement = @"name equals ""John""",
                Context = "NAMECONTEXT2",
                Decision = "NAMEF2OUND",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule1 = rr.RegisterRule(rule1);
            var newRule2 = rr.RegisterRule(rule2);

            var appliedRule = rr.ApplyRules("NAMECONTEXT1", _Data);

            Assert.NotNull(appliedRule);
            Assert.Equal(newRule1.Decision, appliedRule.Decision);
        }

        [Fact]
        public void TestItWorks_String_Equals_MultipleContext2()
        {
            // Rule
            RuleDefinition rule1 = new()
            {
                Number = 1,
                Name = "Name Rule",
                Statement = @"name equals ""John""",
                Context = "NAMECONTEXT1",
                Decision = "NAMEFOUND",
            };

            RuleDefinition rule2 = new()
            {
                Number = 2,
                Name = "Name Rule",
                Statement = @"name equals ""John""",
                Context = "NAMECONTEXT2",
                Decision = "NAMEFOUND2",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule1 = rr.RegisterRule(rule1);
            var newRule2 = rr.RegisterRule(rule2);

            var appliedRule = rr.ApplyRules("NAMECONTEXT2", _Data);

            Assert.NotNull(appliedRule);
            Assert.Equal(newRule2.Decision, appliedRule.Decision);
        }
    }
}