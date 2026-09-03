namespace Aetos.RoslynTesting;

public readonly record struct SourceFile(
    string FileName,
    string Source,
    bool UseMarkup);
