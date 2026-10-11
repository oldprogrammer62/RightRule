using System.Text;

namespace RightRule.LanguageProcessor;

internal static class ArrayLiteralParser
{
    internal static object Parse(string literal)
    {
        if (literal.Length < 2 || literal[0] != '[' || literal[^1] != ']')
            throw new FormatException("Invalid array literal.");

        var elements = SplitElements(literal[1..^1]);
        if (elements.Count == 0 || elements.Any(string.IsNullOrWhiteSpace))
            throw new FormatException("An array must contain one or more elements.");

        string first = elements[0].Trim();

        if (IsQuoted(first))
        {
            return elements.Select(ParseString).ToArray();
        }

        var numbers = elements.Select(ParseNumber).ToArray();
        if (numbers.All(number => number is int))
            return numbers.Cast<int>().ToArray();

        return numbers.Select(Convert.ToDouble).ToArray();
    }

    private static List<string> SplitElements(string contents)
    {
        var elements = new List<string>();
        int elementStart = 0;
        char quote = '\0';

        for (int i = 0; i < contents.Length; i++)
        {
            char current = contents[i];

            if (quote != '\0')
            {
                if (current == '\\' && i + 1 < contents.Length)
                {
                    i++;
                    continue;
                }

                if (current == quote)
                    quote = '\0';

                continue;
            }

            if (current is '\'' or '"')
            {
                quote = current;
            }
            else if (current == ',')
            {
                elements.Add(contents[elementStart..i].Trim());
                elementStart = i + 1;
            }
            else if (current is '[' or ']')
            {
                throw new FormatException("Nested arrays are not supported.");
            }
        }
        if (quote != '\0')
            throw new FormatException("Unterminated string in array literal.");

        elements.Add(contents[elementStart..].Trim());
        return elements;
    }

    private static bool IsQuoted(string value) =>
        value.Length >= 2 &&
        value[0] is '\'' or '"' &&
        value[^1] == value[0];

    private static string ParseString(string element)
    {
        element = element.Trim();

        if (!IsQuoted(element))
            throw new FormatException("Array elements must all be strings or all be numbers.");

        char quote = element[0];
        var result = new StringBuilder();

        for (int i = 1; i < element.Length - 1; i++)
        {
            char current = element[i];

            if (current == '\\' && i + 1 < element.Length - 1 &&
                (element[i + 1] == quote || element[i + 1] == '\\'))
            {
                result.Append(element[++i]);
            }
            else
            {
                result.Append(current);
            }
        }

        return result.ToString();
    }

    private static object ParseNumber(string element)
    {
        if (IsQuoted(element))
            throw new FormatException("Array elements must all be strings or all be numbers.");

        object value = Helper.ParseNumber(element);

        if (value is not (int or double))
            throw new FormatException($"Invalid numeric array element '{element}'.");

        return value;
    }
}
