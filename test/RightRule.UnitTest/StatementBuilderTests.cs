using RightRule.RuleModels;
using System.Text;

namespace RightRuleTests;

public class StatementBuilderTests
{
    private Dictionary<string, object> _OrderData = new Dictionary<string, object>();
    private Dictionary<string, object> _CustomerData = new Dictionary<string, object>();
    private Dictionary<string, object> _StaticData = new Dictionary<string, object>();

    public StatementBuilderTests()
    {
        // Data
        _OrderData = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "Quantity", 100 },
            { "Value", 1500.37 }
        };

        _CustomerData = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "Name", "John Doe" },
            { "Age", 55 },
            { "LengthOfMembership", 15 }
        };

        _StaticData = new Dictionary<string, object>(StringComparer.Ordinal)
        {
            { "LengthOfMembershipThreshold", 10 },
            { "AgeThreshold", 50 },
            { "OrderThreshold", 1000.00 },
            { "CustomerName", "John Doe" }
        };
    }

    [Fact]
    public void ComplexStatement()
    {
        var statement = new StringBuilder();
        statement.Append("CustomerData.LengthOfMembership >= StaticData.LengthOfMembershipThreshold");
        statement.Append(" and ");
        statement.Append("CustomerData.Age >= StaticData.AgeThreshold");
        statement.Append(" and ");
        statement.Append("OrderData.Value >= StaticData.OrderThreshold");
        statement.Append(" or ");
        statement.Append("CustomerData.Name = StaticData.CustomerName");

        // Rule
        DefinedRule rule = new()
        {
            Number = 1,
            Name = "DiscountRule",
            Statement = statement.ToString(),
            Context = "DiscountDecision",
            Decision = "DISCOUNTED",
            FalseDecision = "NOT_DISCOUNTED",
            DecisionData = new Dictionary<string, object>
            {
                { "Discount", 10 }
            }
        };

        BeSoft.IRightRule rr = new BeSoft.RightRule();
        var newRule = rr.RegisterRule(rule);
        rr.RegisterData("OrderData", _OrderData);
        rr.RegisterData("CustomerData", _CustomerData);
        rr.RegisterData("StaticData", _StaticData);

        var appliedRule = rr.ApplyRules("DiscountDecision");

        Assert.Equal(rule.Decision, appliedRule.Decision);
    }
}
