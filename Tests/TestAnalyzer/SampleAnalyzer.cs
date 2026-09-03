using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Aetos.RoslynTesting.Tests.TestAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SampleAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public override void Initialize(
        AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    }

    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
}
