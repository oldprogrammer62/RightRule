using RightRule.DataAdaptors;
using RightRule.Models;
using RightRule.System.Common.DataAdaptor;

namespace RightRule.RightRuleService;

internal sealed partial class RightRuleService : IRightRuleService
{
    public DataAdaptorRegistrationResult RegisterDataAdaptor(IDataAdaptor dataAdaptor)
    {
        if (dataAdaptor == null)
        {
            return new DataAdaptorRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                Message = "DataAdaptor is null"
            };
        }

        if (DataAdaptorService == null)
        {
            return new DataAdaptorRegistrationResult()
            {
                Status = RightRuleStatusEnum.FAILURE,
                Message = "DataAdaptorService is not initialized"
            };
        }
        return DataAdaptorService.RegisterDataAdaptor(dataAdaptor);
    }
}
