using RightRule.System.Common.Operator.Models;

namespace RightRule.OperatorServices.OperatorLoader.CustomOperators
{
    internal interface ICustomOperatorLoader
    {
        Operator? LoadOperator(string PathToOperator);
        List<Operator> LoadOperators(string ConfigurationFilePath);
    }
}