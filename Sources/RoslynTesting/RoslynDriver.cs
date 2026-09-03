using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;

namespace Aetos.RoslynTesting;

public class RoslynDriver
{
    public RoslynDriver()
    {
        this.AssemblyName = "Aetos.RoslynTesting.RoslynDriver.GeneratedAssembly";
    }

    public void AddSource(
        string fileName,
        [StringSyntax("c#-test")] string source,
        bool useMarkup = false)
    {
        ArgumentNullException.ThrowIfNull(fileName);
        ArgumentNullException.ThrowIfNull(source);

        this._sources.Add(new(fileName, source, useMarkup));
    }

    public CSharpParseOptions ParseOptions { get; set; } = CSharpParseOptions.Default;

    public CSharpCompilationOptions CompilationOptions { get; set; } = new(
        OutputKind.DynamicallyLinkedLibrary,
        allowUnsafe: true,
        nullableContextOptions: NullableContextOptions.Enable);

    public ReferenceAssemblies ReferenceAssemblies { get; set; } = ReferenceAssemblies.Net.Net100;

    public IList<MetadataReference> AdditionalReferences { get; } = new List<MetadataReference>();

    public GeneratorDriverOptions GeneratorDriverOptions { get; set; } =
        new(trackIncrementalGeneratorSteps: true);

    public bool Emit { get; set; }

    public EmitOptions EmitOptions { get; set; } = new();

    public void AddAnalyzer(DiagnosticAnalyzer analyzer)
    {
        ArgumentNullException.ThrowIfNull(analyzer);

        this._analyzers.Add(analyzer);
    }

    public void AddSourceGenerator(ISourceGenerator sourceGenerator)
    {
        ArgumentNullException.ThrowIfNull(sourceGenerator);

        this._sourceGenerators.Add(sourceGenerator);
    }

    public void AddSourceGenerator(IIncrementalGenerator sourceGenerator)
    {
        ArgumentNullException.ThrowIfNull(sourceGenerator);

        this.AddSourceGenerator(sourceGenerator.AsSourceGenerator());
    }

    public string AssemblyName { get; set; }

    private readonly List<DiagnosticAnalyzer> _analyzers = new();

    private readonly List<ISourceGenerator> _sourceGenerators = new();

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        using var workspace = new AdhocWorkspace();

        var mainProjectInfo = ProjectInfo.Create(
            ProjectId.CreateNewId(),
            VersionStamp.Default,
            "Main",
            this.AssemblyName,
            LanguageNames.CSharp,
            compilationOptions: this.CompilationOptions);

        var documentInfos = new List<DocumentInfo>();

        foreach (var (fileName, source, useMarkup) in this._sources)
        {
            var inputSource = source;

            if (useMarkup)
            {
                TestFileMarkupParser.GetPositionsAndSpans(
                    inputSource,
                    out var outputSource,
                    out var positions,
                    out var spans);

                inputSource = outputSource;
            }

            var sourceText = SourceText.From(inputSource, Encoding.UTF8, SourceHashAlgorithm.Sha256);
            var textLoader = TextLoader.From(TextAndVersion.Create(sourceText, VersionStamp.Default));

            var documentInfo = DocumentInfo.Create(
                DocumentId.CreateNewId(mainProjectInfo.Id),
                fileName,
                loader: textLoader);

            documentInfos.Add(documentInfo);
        }

        mainProjectInfo = mainProjectInfo.WithDocuments(documentInfos);

        var project = workspace.AddProject(mainProjectInfo);

        var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);

        if (this._sourceGenerators is { Count: > 0 } sourceGenerators)
        {
            var driver = (GeneratorDriver)CSharpGeneratorDriver.Create(
                sourceGenerators,
                parseOptions: this.ParseOptions,
                driverOptions: this.GeneratorDriverOptions);

            driver = driver.RunGenerators(compilation, cancellationToken);

            var runResult = driver.GetRunResult();

            foreach (var result in runResult.Results)
            {
                foreach (var source in result.GeneratedSources)
                {
                    var hintName = source.HintName;
                    var path = Path.GetDirectoryName(hintName);
                    var fileName = Path.GetFileName(hintName);

                    var generatedTextLoader = TextLoader.From(TextAndVersion.Create(source.SourceText, VersionStamp.Default));

                    var generatedDocumentInfo = DocumentInfo.Create(
                        DocumentId.CreateNewId(mainProjectInfo.Id),
                        fileName,
                        loader: generatedTextLoader,
                        isGenerated: true);

                    var updatesSolution = workspace.CurrentSolution.AddDocument(generatedDocumentInfo);
                }
            }
        }
    }

    private readonly List<SourceFile> _sources = new();
}
