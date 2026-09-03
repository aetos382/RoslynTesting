using System.Composition;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeRefactorings;

namespace Aetos.RoslynTesting.Tests.TestCodeRefactoring;

[ExportCodeRefactoringProvider(LanguageNames.CSharp)]
[Shared]
public sealed class SampleCodeRefactoringProvider :
    CodeRefactoringProvider
{
    /// <inheritdoc />
    public override Task ComputeRefactoringsAsync(
        CodeRefactoringContext context)
    {
        return Task.CompletedTask;
    }
}
