using RightRule.Exceptions;
using RightRule.LanguageProcessor.Parser;
using RightRule.RegistrationServices.DataRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

internal static class EvaluatorHelper
{
    internal static object? ResolveOperandValue(Operand Op, IDataRegistrationService DataService)
    {
        try
        {
            return ResolveOperand(Op, DataService);
        }
        catch
        {
            throw;
        }
    }

    internal static object? ResolveVariable(string Identifier, IDataRegistrationService DataService)
    {
        var variableValue = GetValue(Identifier, Identifier, DataService);
        return variableValue;
    }

    internal static object? ResolveIdentifier(string Identifier, IDataRegistrationService DataService)
    {
        if (string.IsNullOrEmpty(Identifier))
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' cannot be null or empty.");
        }
        if (Identifier.Split('.').Length != 2)
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' must be in the format 'DatasetName.IdentifierName'.");
        }

        var split = Identifier.Split('.');
        var parent = split[0];
        var child = split[1];

        var identifierValue = GetValue(parent, child, DataService);
        return identifierValue;
    }

    private static object? GetValue(string Left, string Right, IDataRegistrationService DataService)
    {
        return DataService.GetDataItemValue(Left, Right);
    }

    private static object? ResolveOperand(Operand Op, IDataRegistrationService DataService)
    {
        try
        {
            object opValue = GetOperator(Op);

            return Op.Kind switch
            {
                OperandKindEnum.VARIABLE => opValue is string vVar ? ResolveVariable(vVar, DataService) : ResolveVariable(Op.Raw, DataService),
                OperandKindEnum.IDENTIFIER => opValue is string sId ? ResolveIdentifier(sId, DataService) : ResolveIdentifier(Op.Raw, DataService),
                OperandKindEnum.NUMBER => opValue is string sNum ? ParseNumber(sNum) : opValue,
                OperandKindEnum.BOOLEAN => opValue is string sBool ? ParseBoolean(sBool) : opValue,
                OperandKindEnum.STRING => opValue is string s ? s : opValue?.ToString() ?? string.Empty,
                _ => opValue
            };
        }
        catch
        {
            throw;
        }
    }

    private static string GetOperator(Operand Op)
    {
        try
        {
            return Op.Raw;
        }
        catch
        {
            throw;
        }
    }

    private static object ParseBoolean(string Value)
    {
        try
        {
            return Helper.ParseBoolean(Value);
        }
        catch
        {
            throw;
        }
    }

    private static object ParseNumber(string Value)
    {
        try
        {
            return Helper.ParseNumber(Value);
        }
        catch
        {
            throw;
        }
    }
}
