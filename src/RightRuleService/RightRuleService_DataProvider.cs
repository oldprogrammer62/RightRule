using RightRule.DataProvider;
using RightRule.Models;
using RightRule.System.Common.DataProvider;

namespace RightRule.RightRuleService;

internal partial class RightRuleService : IRightRuleService
{
    public DataProviderRegistrationResult RegisterDataProvider(IDataProvider dataProvider)
    {
        if (dataProvider == null)
        {
            return new DataProviderRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                Message = "DataProvider is null"
            };
        }

        if (DataProviderService == null)
        {
            return new DataProviderRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                Message = "DataProviderService is not initialized"
            };
        }
        return DataProviderService.RegisterDataProvider(dataProvider);
    }
}
