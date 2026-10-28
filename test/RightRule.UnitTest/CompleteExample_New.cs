using RightRule.Models;
using RightRule.RuleServices.RuleModels;
using RightRuleLogger;

namespace RightRuleTests;

public class CompleteExample_New
{
    /// <summary>
    /// This test is not intended to prove anything.
    /// It is designed to show the entire set up and execute process for RightRule
    /// </summary>
    [Fact]
    public void ApplyOrderDiscount_WithCustomLogger()
    {
        // These 2 variables drive the rules process.
        // They describe the customers current order situation
        //  Order Value
        //  Previous Orders
        decimal orderValue = 200m;
        int previousOrders = 5;

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
        };
        RightRuleLogger<RightRule.RightRule> logger = new RightRuleLogger<RightRule.RightRule>();

        using var rr = new RightRule.RightRule(options, logger);

        // This informs RightRule the event that is being processed.
        const string context = "OrderDiscount";

        rr.RegisterData("Order", new Dictionary<string, object>
        {
            ["Value"] = orderValue,
            ["PreviousOrders"] = previousOrders
        });

        rr.RegisterData("OrderThresholds", new Dictionary<string, object>
        {
            ["LoyaltyValue"] = 100m,
            ["ReturningValue"] = 50m,
            ["LoyaltyPreviousOrders"] = 5,
            ["ReturningPreviousOrders"] = 2
        });

        rr.RegisterData("Discount", new Dictionary<string, object>
        {
            ["Default"] = 2,
            ["GoodWill"] = 5,
            ["Returning"] = 10,
            ["Loyalty"] = 15
        });

        var newDefaultRule = new DefaultRule()
        {
            Name = "DefaultCustomer",
            Context = context,
            TrueDecision = "NoDiscount",
            TrueDecisionData = """{"discount":"$Discount.Default%"}"""
        };
        rr.RegisterDefaultRule(newDefaultRule);

        rr.RegisterRules(new List<DefinedRule>
        {
            new()
            {
                Number = 1,
                Name = "NewCustomer",
                Context = context,
                Statement = "Order.Value >= OrderThresholds.ReturningValue and Order.PreviousOrders < OrderThresholds.ReturningPreviousOrders",
                TrueDecision = "Discount",
                TrueDecisionData = """{"discount":"$Discount.GoodWill%"}""",
                FalseDecision = "Ineligible"
            },
            new()
            {
                Number = 2,
                Name = "LoyalCustomer",
                Context = context,
                Statement = "Order.Value >= OrderThresholds.LoyaltyValue and Order.PreviousOrders >= OrderThresholds.LoyaltyPreviousOrders",
                TrueDecision = "Discount",
                TrueDecisionData = """{"discount":"$Discount.Loyalty%"}""",
                FalseDecision = "Ineligible"
            },
            new()
            {
                Number = 3,
                Name = "ReturningCustomer",
                Context = context,
                Statement = "Order.Value >= OrderThresholds.ReturningValue and Order.PreviousOrders >= OrderThresholds.ReturningPreviousOrders",
                TrueDecision = "Discount",
                TrueDecisionData = """{"discount":"$Discount.Returning%"}""",
                FalseDecision = "Ineligible"
            }
        });

        var result = rr.ApplyRule(context);
        Assert.NotNull(result);
    }

    [Fact]
    public void ApplyOrderDiscount_NoLogger()
    {
        // These 2 variables drive the rules process.
        // They describe the customers current order situation
        //  Order Value
        //  Previous Orders
        decimal orderValue = 200m;
        int previousOrders = 5;

        RightRuleOptions options = new RightRuleOptions()
        {
            DataCheck = false,
            Diagnostics = true,
            LoadCustomOperators = true,
            LoadDefaultOperators = true
            // options.CustomOperatorLibraryPath = "Hello/world"; // Use default
        };
        using var rr = new RightRule.RightRule(options);

        // This informs RightRule the event that is being processed.
        const string context = "OrderDiscount";

        rr.RegisterData("Order", new Dictionary<string, object>
        {
            ["Value"] = orderValue,
            ["PreviousOrders"] = previousOrders
        });

        rr.RegisterData("OrderThresholds", new Dictionary<string, object>
        {
            ["LoyaltyValue"] = 100m,
            ["ReturningValue"] = 50m,
            ["LoyaltyPreviousOrders"] = 5,
            ["ReturningPreviousOrders"] = 2
        });

        rr.RegisterData("Discount", new Dictionary<string, object>
        {
            ["Default"] = 2,
            ["GoodWill"] = 5,
            ["Returning"] = 10,
            ["Loyalty"] = 15
        });

        var newDefaultRule = new DefaultRule()
        {
            Name = "DefaultCustomer",
            Context = context,
            TrueDecision = "NoDiscount",
            TrueDecisionData = """{"discount":"$Discount.Default%"}"""
        };
        rr.RegisterDefaultRule(newDefaultRule);

        rr.RegisterRules(new List<DefinedRule>
        {
            new()
            {
                Number = 1,
                Name = "NewCustomer",
                Context = context,
                Statement = "Order.Value >= OrderThresholds.ReturningValue and Order.PreviousOrders < OrderThresholds.ReturningPreviousOrders",
                TrueDecision = "Discount",
                TrueDecisionData = """{"discount":"$Discount.GoodWill%"}""",
                FalseDecision = "Ineligible"
            },
            new()
            {
                Number = 2,
                Name = "LoyalCustomer",
                Context = context,
                Statement = "Order.Value >= OrderThresholds.LoyaltyValue and Order.PreviousOrders >= OrderThresholds.LoyaltyPreviousOrders",
                TrueDecision = "Discount",
                TrueDecisionData = """{"discount":"$Discount.Loyalty%"}""",
                FalseDecision = "Ineligible"
            },
            new()
            {
                Number = 3,
                Name = "ReturningCustomer",
                Context = context,
                Statement = "Order.Value >= OrderThresholds.ReturningValue and Order.PreviousOrders >= OrderThresholds.ReturningPreviousOrders",
                TrueDecision = "Discount",
                TrueDecisionData = """{"discount":"$Discount.Returning%"}""",
                FalseDecision = "Ineligible"
            }
        });

        var result = rr.ApplyRule(context);
        Assert.NotNull(result);
    }
}
