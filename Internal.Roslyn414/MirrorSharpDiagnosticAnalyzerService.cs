using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace MirrorSharp.Internal.Roslyn414;

[Shared]
[Export(typeof(IDiagnosticAnalyzerService))]
internal class MirrorSharpDiagnosticAnalyzerService : IDiagnosticAnalyzerService
{
    public void RequestDiagnosticRefresh() => throw new NotImplementedException();

    public Task<ImmutableArray<DiagnosticData>> ForceAnalyzeProjectAsync(Project project, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<ImmutableArray<DiagnosticData>> GetDiagnosticsForIdsAsync(Project project, DocumentId? documentId, ImmutableHashSet<string>? diagnosticIds, Func<DiagnosticAnalyzer, bool>? shouldIncludeAnalyzer, bool includeLocalDocumentDiagnostics, bool includeNonLocalDocumentDiagnostics, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<ImmutableArray<DiagnosticData>> GetProjectDiagnosticsForIdsAsync(Project project, ImmutableHashSet<string>? diagnosticIds, Func<DiagnosticAnalyzer, bool>? shouldIncludeAnalyzer, bool includeNonLocalDocumentDiagnostics, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<ImmutableArray<DiagnosticData>> GetDiagnosticsForSpanAsync(TextDocument document, TextSpan? range, Func<string, bool>? shouldIncludeDiagnostic, ICodeActionRequestPriorityProvider priorityProvider, DiagnosticKind diagnosticKind, bool isExplicit, CancellationToken cancellationToken)  => throw new NotImplementedException();

    public DiagnosticAnalyzerInfoCache AnalyzerInfoCache => throw new NotImplementedException();
}
