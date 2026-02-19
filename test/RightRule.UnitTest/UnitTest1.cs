using RightRule.Rules;

namespace BSoftTests
{
    public class UnitTest1
    {
        private readonly Dictionary<string, object> _Data;

        public UnitTest1()
        {
            // Data
            _Data = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                { "name", "Johnathan" },
                { "fullname", "Johnathan Cassar" },
                { "age", 28 },
                { "role", "nuser" }
            };
        }

        [Fact]
        public void TestItWorks_NoDecision()
        {
            // Rule
            Rule rule = new()
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

            Assert.Equal(ParsedRule.DefaultRule().Decision, appliedRule.Decision);
        }

        [Fact]
        public void TestItWorks_Number_Equals()
        {
            // Rule
            Rule rule = new()
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
        public void TestItWorks_String_Equals()
        {
            // Rule
            Rule rule = new()
            {
                Number = 2,
                Name = "Name Rule",
                Statement = @"name equals ""Johnathan""",
                Context = "NameContext",
                Decision = "OLDENOUGH",
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
            Rule rule = new()
            {
                Number = 3,
                Name = "Name Rule",
                Statement = "name equals fullname",
                Context = "NameContext",
                Decision = "OLDENOUGH",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule = rr.RegisterRule(rule);

            var appliedRule = rr.ApplyRules("NameContext", _Data);

            Assert.Equal(ParsedRule.DefaultRule().Decision, appliedRule.Decision);
        }

        [Fact]
        public void TestItWorks_String_Equals_MultipleContext1()
        {
            // Rule
            Rule rule1 = new()
            {
                Number = 1,
                Name = "Name Rule",
                Statement = @"name equals ""Johnathan""",
                Context = "Context1",
                Decision = "FOUND1",
            };

            Rule rule2 = new()
            {
                Number = 2,
                Name = "Name Rule",
                Statement = @"name equals ""Johnathan""",
                Context = "Context2",
                Decision = "FOUND2",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule1 = rr.RegisterRule(rule1);
            var newRule2 = rr.RegisterRule(rule2);

            var appliedRule = rr.ApplyRules("Context1", _Data);

            Assert.NotNull(appliedRule);
            Assert.Equal(newRule1, appliedRule);
        }

        [Fact]
        public void TestItWorks_String_Equals_MultipleContext2()
        {
            // Rule
            Rule rule1 = new()
            {
                Number = 1,
                Name = "Name Rule",
                Statement = @"name equals ""Johnathan""",
                Context = "Context1",
                Decision = "FOUND1",
            };

            Rule rule2 = new()
            {
                Number = 2,
                Name = "Name Rule",
                Statement = @"name equals ""Johnathan""",
                Context = "Context2",
                Decision = "FOUND2",
            };

            RightRule.RightRule rr = new RightRule.RightRule();
            var newRule1 = rr.RegisterRule(rule1);
            var newRule2 = rr.RegisterRule(rule2);

            var appliedRule = rr.ApplyRules("Context2", _Data);

            Assert.NotNull(appliedRule);
            Assert.Equal(newRule2, appliedRule);
        }
    }
}