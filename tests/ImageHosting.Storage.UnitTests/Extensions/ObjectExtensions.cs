using System.Reflection;
using Xunit.Abstractions;

namespace ImageHosting.Storage.UnitTests.Extensions;

public static class ObjectExtensions
{
    public static void PrintProperties(this ITestOutputHelper outputHelper, object? obj)
    {
        if (obj == null)
        {
            outputHelper.WriteLine("null");
            return;
        }

        var type = obj.GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var value = property.GetValue(obj) ?? "null";
            outputHelper.WriteLine($"{property.Name}: {value}");
        }
    }
}
