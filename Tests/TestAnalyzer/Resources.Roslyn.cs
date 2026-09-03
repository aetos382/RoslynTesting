using Microsoft.CodeAnalysis;

namespace Aetos.RoslynTesting.Tests.TestAnalyzer;

internal partial class Resources
{
    internal static LocalizableResourceString GetLocalizableResourceString(string name)
    {
        return new LocalizableResourceString(name, ResourceManager, typeof(Resources));
    }
}
