using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices.OperatorLoader.DefaultOperators
{
    internal interface IDefaultOperatorLoader
    {
        List<Operator> Load();
    }
}