using RightRule.Data;
using RightRule.StatementProcessor.Parser;
using RightRule.System.Common;
using RightRule.System.Common.Exceptions;
using System.Data;
using System.Globalization;

namespace RightRule.StatementProcessor.Evaluator;

internal class EvaluatorHelper
{
    internal static object? ResolveIdentifier(RegisteredDatasets? Datasets, RegisteredDataObjects? DataObjects, string Identifier)
    {
        if (string.IsNullOrEmpty(Identifier))
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' cannot be null or empty.");
        }
        if (Identifier.Split('.').Length != 2)
        {
            throw new InvalidIdentifierException(Identifier, -1, $"Identifier '{Identifier}' must be in the format 'datasetName.identifierName'.");
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

    private static object? ResolveIdentifier(RegisteredDataObjects? DataObjects, string ObjectName, string PropertyName)
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

        return EvaluatorHelper.GetValue(DataObjects, ObjectName, PropertyName);
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
        if (!Dataset.Data.TryGetValue(KeyName, out _))
        {
            throw new KeyNotFoundException($"Unknown variable '{KeyName}' in Registered Dataset {Dataset.Name}.");
        }

        Dataset.Data.TryGetValue(KeyName, out var result);
        return result!;
    }

    internal static object? GetValue(RegisteredDataObjects DataObjects, string objectName, string propertyName)
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

        var objectReference = ConvertObject(dataObject);
        if (objectReference == null)
        {
            return null;
        }

        //var type = GetType(objectReference, objectName);
        //if (type == null)
        //{
        //    return null;
        //}

        return GetPropertyValue(objectReference.Data, propertyName);
    }

    private static RegisteredDataObject? ConvertObject(object? obj)
    {
        try
        {
            return obj as RegisteredDataObject;
        }
        catch
        {
            return null;
        }
    }

    private static ObjectReference? GetType(ObjectReferences? objectReferences, string objectName)
    {
        if (objectReferences == null)
        {
            return null;
        }
        var type = objectReferences.Objects.FirstOrDefault(t => t.Name.Equals(objectName, StringComparison.OrdinalIgnoreCase));
        return type;
    }

    private static object? GetPropertyValue(object obj, string propertyName)
    {
        var type = obj.GetType();
        var propertyInfo = type.GetProperty(propertyName);
        if (propertyInfo is not null)
        {
            return propertyInfo.GetValue(obj);
        }
        return null;
    }

    internal static object? ResolveOperandValue(RegisteredDatasets? Datasets, RegisteredDataObjects? DataObjects, Operand op)
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

    private static object? ResolveOperand(RegisteredDatasets? Datasets, RegisteredDataObjects? DataObjects, Operand op)
    {
        try
        {
            object opValue = ResolveOperatorValue(op);

            return op.Kind switch
            {
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

    internal static string FormatValue(object? value)
    {
        if (value is null)
            return "null";

        if (value is string s)
        {
            // Escape backslashes and quotes
            var escaped = s.Replace("\\", "\\\\").Replace("\"", "\\\"");
            return $"\"{escaped}\"";
        }

        if (value is bool b)
            return b ? "true" : "false";

        if (value is IFormattable f)
            return f.ToString(null, CultureInfo.InvariantCulture) ?? value.ToString() ?? string.Empty;

        return value.ToString() ?? string.Empty;
    }
}
