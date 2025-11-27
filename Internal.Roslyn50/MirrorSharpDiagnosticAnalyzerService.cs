using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace MirrorSharp.Internal.Roslyn50;

[Shared]
[Export(typeof(IDiagnosticAnalyzerService))]
internal class MirrorSharpDiagnosticAnalyzerService : IDiagnosticAnalyzerService
{
    public void RequestDiagnosticRefresh()
    {
        throw new NotSupportedException();
    }

    public Task<ImmutableArray<DiagnosticData>> ForceRunCodeAnalysisDiagnosticsAsync(Project project, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task<bool> IsAnyDiagnosticIdDeprioritizedAsync(Project project, ImmutableArray<string> diagnosticIds, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task<ImmutableArray<DiagnosticData>> GetDiagnosticsForIdsAsync(Project project, ImmutableArray<DocumentId> documentIds, ImmutableHashSet<string>? diagnosticIds, AnalyzerFilter analyzerFilter, bool includeLocalDocumentDiagnostics, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task<ImmutableArray<DiagnosticData>> GetProjectDiagnosticsForIdsAsync(Project project, ImmutableHashSet<string>? diagnosticIds, AnalyzerFilter analyzerFilter, CancellationToken cancellationToken) {

        throw new NotSupportedException();
    }

    public Task<ImmutableArray<DiagnosticData>> GetDiagnosticsForSpanAsync(TextDocument document, TextSpan? range, DiagnosticIdFilter diagnosticIdFilter, CodeActionRequestPriority? priority, DiagnosticKind diagnosticKind, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task<ImmutableDictionary<string, ImmutableArray<DiagnosticDescriptor>>> GetDiagnosticDescriptorsPerReferenceAsync(Solution solution, ProjectId? projectId, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task<ImmutableArray<DiagnosticDescriptor>> GetDiagnosticDescriptorsAsync(Solution solution, ProjectId projectId, AnalyzerReference analyzerReference, string language, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }

    public Task<ImmutableArray<string>> GetCompilationEndDiagnosticDescriptorIdsAsync(Solution solution, CancellationToken cancellationToken)
    {
        throw new NotSupportedException();
    }
}