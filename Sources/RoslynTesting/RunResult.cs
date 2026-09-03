using System.Collections.Immutable;

using Microsoft.CodeAnalysis.Testing;

namespace Aetos.RoslynTesting;

public sealed class RunResult
{
    internal RunResult(
        ImmutableArray<DiagnosticResult> expectedDiagnostics)
    {
        this.ExpectedDiagnostics = expectedDiagnostics;
    }

    public ImmutableArray<DiagnosticResult> ExpectedDiagnostics { get; }
}
