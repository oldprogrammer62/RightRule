using RightRule.DataRegistration;
using RightRule.StatementProcessor.Parser;
using RightRule.System.Common.Exceptions;

namespace RightRule.StatementProcessor.Evaluator;

internal static class EvaluatorHelper
{
    internal static object? ResolveVariable(IRegisteredDataObjects? DataObjects, string Identifier)
    {
        if (DataObjects is null)
        {
            return null;
        }

        var dataObject = DataObjects.GetData(Identifier);
        if (dataObject is null)
        {
            return null;
        }

        return GetPropertyValue(Identifier, dataObject.Value);
    }

    internal static object? ResolveIdentifier(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, string Identifier)
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
        var ds = Datasets?.GetData(parent);
        if (ds is not null)
        {
            return ResolveIdentifier(ds, child);
        }
        return ResolveIdentifier(DataObjects, parent, child);
    }

    internal static object? ResolveOperandValue(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Operand op)
    {
        try
        {
            return ResolveOperand(Datasets, DataObjects, op);
        }
        catch
        {
            throw;
        }
    }

    private static object? ResolveIdentifier(IRegisteredDataObjects? DataObjects, string ObjectName, string PropertyName)
    {
        if (string.IsNullOrEmpty(ObjectName))
        {
            throw new KeyNotFoundException($"Unknown Object.");
        }
        if (string.IsNullOrEmpty(PropertyName))
        {
            throw new KeyNotFoundException($"Unknown Property.");
        }
        if (DataObjects is null)
        {
            throw new KeyNotFoundException($"Unknown dataobject '{ObjectName}'");
        }

        return GetValue(DataObjects, ObjectName, PropertyName);
    }

    private static object? ResolveIdentifier(RegisteredDataset? Dataset, string KeyName)
    {
        if (string.IsNullOrEmpty(KeyName))
        {
            throw new KeyNotFoundException($"Unknown variable '{KeyName}'.");
        }
        if (Dataset is null)
        {
            throw new KeyNotFoundException($"Unknown dataset '{Dataset}'");
        }
        if (!Dataset.Value.TryGetValue(KeyName, out _))
        {
            throw new KeyNotFoundException($"Unknown variable '{KeyName}' in Registered Dataset {Dataset.Name}.");
        }

        Dataset.Value.TryGetValue(KeyName, out var result);
        return result!;
    }

    private static object? GetValue(IRegisteredDataObjects DataObjects, string objectName, string propertyName)
    {
        if (DataObjects is null)
        {
            return null;
        }
        if (DataObjects.Objects is null || DataObjects.Objects.Count == 0)
        {
            return null;
        }

        // Get specified object from RegisteredDataObjects
        var dataObject = DataObjects.GetData(objectName);
        if (dataObject is null)
        {
            return null;
        }

        return GetPropertyValue(propertyName, dataObject.Value);
    }

    private static object? GetPropertyValue(string propertyName, object? propertyValue)
    {
        if (propertyValue is null)
        {
            return null;
        }

        var type = propertyValue.GetType();

        // Return the object itself if it is a primitive type, enum, string, or decimal
        if (type.IsPrimitive || type.IsEnum || propertyValue is string || propertyValue is decimal || propertyValue is DateTime)
        {
            return propertyValue;
        }

        if (string.IsNullOrEmpty(propertyName))
        {
            return null;
        }

        return GetPropertyValue(type, propertyName, propertyValue);
    }

    private static object? GetPropertyValue(Type type, string PropertyName, object PropertyValue)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTuple<,>))
        {
            return GetValueFromTuple(type, PropertyName, PropertyValue);
        }

        var propertyInfo = type.GetProperty(PropertyName);
        return propertyInfo?.GetValue(PropertyValue);
    }

    private static object? GetValueFromTuple(Type type, string PropertyName, object PropertyValue)
    {
        var fieldInfo = type.GetField(PropertyName);
        if (fieldInfo != null)
        {
            return fieldInfo.GetValue(PropertyValue);
        }

        // Use default Field Name
        fieldInfo = type.GetField("Item2");
        if (fieldInfo != null)
        {
            return fieldInfo.GetValue(PropertyValue);
        }
        return default;
    }

    private static object? ResolveOperand(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Operand op)
    {
        try
        {
            object opValue = ResolveOperatorValue(op);

            return op.Kind switch
            {
                OperandKind.Variable => opValue is string vVar ? ResolveVariable(DataObjects, vVar) : ResolveVariable(DataObjects, op.Raw),
                OperandKind.Identifier => opValue is string sId ? ResolveIdentifier(Datasets, DataObjects, sId) : ResolveIdentifier(Datasets, DataObjects, op.Raw),
                OperandKind.Number => opValue is string sNum ? ParseNumber(sNum) : opValue,
                OperandKind.Boolean => opValue is string sBool ? ParseBoolean(sBool) : opValue,
                OperandKind.String => opValue is string s ? s : (opValue?.ToString() ?? string.Empty),
                _ => opValue
            };
        }
        catch
        {
            throw;
        }
    }

    private static string ResolveOperatorValue(Operand operand)
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
