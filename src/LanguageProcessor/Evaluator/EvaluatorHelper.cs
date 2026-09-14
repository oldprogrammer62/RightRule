using RightRule.Exceptions;
using RightRule.LanguageProcessor.Parser;
using RightRule.RegistrationServices.DataRegistration;

namespace RightRule.LanguageProcessor.Evaluator;

internal static class EvaluatorHelper
{
    internal static object? ResolveVariable(IRegisteredDataObjects? DataObjects, string Identifier, InewGetDataService getDataService)
    {
        if (DataObjects is null)
        {
            return null;
        }

        // TO DO
        var newValue = NewGetValue(getDataService, Identifier, Identifier);
        return newValue;

        //var dataObject = DataObjects.GetData(Identifier);
        //if (dataObject is null)
        //{
        //    return null;
        //}

        //return GetPropertyValue(Identifier, dataObject.Value);
    }

    internal static object? ResolveIdentifier(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, string Identifier, InewGetDataService getDataService)
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

        // TO DO
        var newValue = NewGetValue(getDataService, parent, child);
        return newValue;

        //var ds = Datasets?.GetData(parent);
        //if (ds is not null)
        //{
        //    return ResolveIdentifier(ds, child);
        //}
        //return ResolveIdentifier(DataObjects, parent, child, getDataService);
    }

    private static object? NewGetValue(InewGetDataService getDataService, string LeftSide, string RightSide)
    {
        var value = getDataService.newGetDataValue(LeftSide, RightSide);
        return value;
    }

    internal static object? ResolveOperandValue(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Operand op, InewGetDataService getDataService)
    {
        try
        {
            return ResolveOperand(Datasets, DataObjects, op, getDataService);
        }
        catch
        {
            throw;
        }
    }

    private static object? ResolveIdentifier(IRegisteredDataObjects? DataObjects, string ObjectName, string PropertyName, InewGetDataService getDataService)
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

        return GetValue(DataObjects, ObjectName, PropertyName, getDataService);
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
        if (!Dataset.Values.TryGetValue(KeyName, out _))
        {
            throw new KeyNotFoundException($"Unknown variable '{KeyName}' in Registered Dataset {Dataset.Name}.");
        }

        Dataset.Values.TryGetValue(KeyName, out var result);
        return result!;
    }

    private static object? GetValue(IRegisteredDataObjects DataObjects, string objectName, string propertyName, InewGetDataService getDataService)
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
        if (
            type.IsPrimitive || 
            propertyValue is string || 
            propertyValue is decimal || 
            propertyValue is DateTime || 
            propertyValue is DateOnly ||
            propertyValue is TimeOnly
            )
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

    private static object? ResolveOperand(IRegisteredDatasets? Datasets, IRegisteredDataObjects? DataObjects, Operand op, InewGetDataService getDataService)
    {
        try
        {
            // TO DO Resolve var
            object opValue = ResolveOperatorValue(op);

            return op.Kind switch
            {
                OperandKind.Variable => opValue is string vVar ? ResolveVariable(DataObjects, vVar, getDataService) : ResolveVariable(DataObjects, op.Raw, getDataService),
                OperandKind.Identifier => opValue is string sId ? ResolveIdentifier(Datasets, DataObjects, sId, getDataService) : ResolveIdentifier(Datasets, DataObjects, op.Raw, getDataService),
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
