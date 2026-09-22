using RightRule.Models.RuleModels;
using RightRule.ServiceRegister;

namespace RightRule.RegistrationServices.RuleRegistration.RuleBuilders;

public static class RuleBuilderExtensions
{
    internal static RuleRegistrationResult? Register(this DefinedRule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);
        try
        {
            var service = ServiceLocator.GetRequiredService<IRuleRegistrationService>();
            if (service == null)
            {
                return default;
            }
            return service.RegisterRule(rule);
        }
        catch (InvalidOperationException)
        {
            return default;
        }
    }
}