using RightRule.Exceptions;
using RightRule.LanguageProcessor.Parser;
using RightRule.RegistrationServices.DataRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

internal static class EvaluatorHelper
{
    internal static object? ResolveOperandValue(Operand op, IDataService dataService)
    {
        try
        {
            return ResolveOperand(op, dataService);
        }
        catch
        {
            throw;
        }
    }

    internal static object? ResolveVariable(string Identifier, IDataService dataService)
    {
        var variableValue = GetValue(Identifier, Identifier, dataService);
        return variableValue;
    }

    internal static object? ResolveIdentifier(string Identifier, IDataService dataService)
    {
        if (string.IsNullOrEmpty(Identifier))
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' cannot be null or empty.");
        }
        if (Identifier.Split('.').Length != 2)
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' must be in the format 'DataName.identifierName'.");
        }

        var split = Identifier.Split('.');
        var parent = split[0];
        var child = split[1];

        var identifierValue = GetValue(parent, child, dataService);
        return identifierValue;
    }

    private static object? GetValue(string LeftSide, string RightSide, IDataService dataService)
    {
        return dataService.GetDataValue(LeftSide, RightSide);
    }

    private static object? ResolveOperand(Operand op, IDataService dataService)
    {
        try
        {
            object opValue = GetOperator(op);

            return op.Kind switch
            {
                OperandKind.Variable => opValue is string vVar ? ResolveVariable(vVar, dataService) : ResolveVariable(op.Raw, dataService),
                OperandKind.Identifier => opValue is string sId ? ResolveIdentifier(sId, dataService) : ResolveIdentifier(op.Raw, dataService),
                OperandKind.Number => opValue is string sNum ? ParseNumber(sNum) : opValue,
                OperandKind.Boolean => opValue is string sBool ? ParseBoolean(sBool) : opValue,
                OperandKind.String => opValue is string s ? s : opValue?.ToString() ?? string.Empty,
                _ => opValue
            };
        }
        catch
        {
            throw;
        }
    }

    private static string GetOperator(Operand operand)
    {
        try
        {
            return operand.Raw;
        }
        catch
        {
            throw;
        }
    }

    private static object ParseBoolean(string raw)
    {
        try
        {
            return Helper.ParseBoolean(raw);
        }
        catch
        {
            throw;
        }
    }

    private static object ParseNumber(string raw)
    {
        try
        {
            return Helper.ParseNumber(raw);
        }
        catch
        {
            throw;
        }
    }
}
