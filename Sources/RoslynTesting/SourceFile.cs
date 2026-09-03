using Microsoft.CodeAnalysis.Text;

namespace Aetos.RoslynTesting;

public readonly record struct SourceFile(
    string FileName,
    SourceText Source);
