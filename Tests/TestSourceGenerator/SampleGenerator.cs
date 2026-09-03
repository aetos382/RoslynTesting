using Microsoft.CodeAnalysis;

namespace Aetos.RoslynTesting.Tests.TestSourceGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class SampleGenerator :
    IIncrementalGenerator
{
    /// <inheritdoc />
    void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
    {
    }
}
