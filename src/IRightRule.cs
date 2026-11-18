using RightRule.RightRuleSession;

namespace RightRule;

internal interface IRightRule
{
    IRightRuleSession CreateSession();
}
