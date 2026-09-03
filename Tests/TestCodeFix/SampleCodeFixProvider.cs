using System.Collections.Immutable;
using System.Composition;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Aetos.RoslynTesting.Tests.TestCodeFix;

[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public sealed class SampleCodeFixProvider :
    CodeFixProvider
{
    /// <inheritdoc />
    public override Task RegisterCodeFixesAsync(
        CodeFixContext context)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds { get; }
}
