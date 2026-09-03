using Microsoft.CodeAnalysis;

namespace Aetos.RoslynTesting.Tests.TestAnalyzer;

internal partial class Resources
{
    public static LocalizableResourceString GetLocalizableResourceString(string name)
    {
        return new LocalizableResourceString(name, ResourceManager, typeof(Resources));
    }
}
